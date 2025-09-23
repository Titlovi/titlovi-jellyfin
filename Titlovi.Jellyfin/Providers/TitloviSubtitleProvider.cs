using System.Globalization;
using MediaBrowser.Controller.MediaEncoding;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using Titlovi.Jellyfin.Extensions;
using Titlovi.Jellyfin.Interfaces;
using Titlovi.Jellyfin.Models.Subtitle;

namespace Titlovi.Jellyfin.Providers;

/// <summary>
/// Actual provider that gives jellyfin
/// access to subtitles from titlovi.com.
/// </summary>
public class TitloviSubtitleProvider : ISubtitleProvider
{
    private readonly ILogger<TitloviSubtitleProvider> logger;
    private readonly ITitloviManager titloviManager;
    private readonly IMediaEncoder mediaEncoder;

    public TitloviSubtitleProvider(ILogger<TitloviSubtitleProvider> logger, ITitloviManager titloviManager, IMediaEncoder mediaEncoder)
    {
        this.logger = logger;
        this.titloviManager = titloviManager;
        this.mediaEncoder = mediaEncoder;
    }

    /// <inheritdoc />
    public string Name => "Titlovi.com";

    /// <inheritdoc />
    public IEnumerable<VideoContentType> SupportedMediaTypes => new[]
    {
      VideoContentType.Episode,
      VideoContentType.Movie
    };

    /// <inheritdoc />
    public async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        var idParts = id.Split('-');
        if (idParts.Length < 3)
        {
            throw new FormatException($"Invalid subtitle id format: {id}");
        }

        var responseBytes = await titloviManager.DownloadAsync(new SubtitleDownload()
        {
            MediaId = Convert.ToInt32(idParts[0], CultureInfo.InvariantCulture),
            Type = Convert.ToInt32(idParts[1], CultureInfo.InvariantCulture)
        }).ConfigureAwait(false);

        if (responseBytes is null)
        {
            throw new HttpRequestException($"Failed to download subtitle from id: {id}");
        }

        var subtitles = titloviManager.ExtractSubtitles(responseBytes);
        if (subtitles.Count == 0)
        {
            throw new InvalidDataException("Data received didn't contain any subtitles!");
        }

        logger.LogInformation("Number of subtitles detected in stream: {Count}", subtitles.Count);

        return new SubtitleResponse()
        {
            Format = "srt",
            IsForced = false,
            IsHearingImpaired = false,
            Language = idParts[2].FromProviderLanguage(),
            Stream = new MemoryStream(subtitles[0]),
        };
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        var tokenInfo = TitloviJellyfin.Instance?.Configuration.TokenInfo;
        var imdbId = request.ProviderIds?.GetValueOrDefault("Imdb");

        if (tokenInfo is null || string.IsNullOrEmpty(imdbId))
        {
            return new List<RemoteSubtitleInfo>();
        }

        var response = await titloviManager.SearchAsync(new SubtitleSearch()
        {
            UserId = tokenInfo.UserId,
            Token = tokenInfo.Token.ToString(),
            ImdbId = request.ParentIndexNumber is null ? imdbId : null,
            Query = request.ParentIndexNumber is not null ? request.SeriesName : null,
            Type = request.ParentIndexNumber is not null ? 2 : 1,
            Lang = request.Language.ToProviderLanguage(),
            IgnoreLangAndEpisode = false,
            Season = request.ParentIndexNumber,
            Episode = request.IndexNumber
        }).ConfigureAwait(false);

        if (response is null)
        {
            return new List<RemoteSubtitleInfo>();
        }

        // TODO :: currently only the first page of the subtitles
        // that are returned is scanned, that is an issue. Looping
        // through pages and getting all the subtitles is going to be needed.

        // TODO :: the results should also take into considaration the media
        // information from `GetMediaStreamsAsync`. (such as the `encoder`, `resolution`...)

        return response.Results.Select(result => new RemoteSubtitleInfo()
        {
            Id = $"{result.Id}-{result.Type}-{result.Language}",
            Name = result.Title,
            CommunityRating = result.Rating,
            DownloadCount = result.DownloadCount,
            ProviderName = Name,
            AiTranslated = false,
            DateCreated = result.Date,
            Format = "srt",
            Author = string.Empty,
            Comment = result.Release,
            ThreeLetterISOLanguageName = result.Language.FromProviderLanguage()
        }).OrderByDescending(result => result.DownloadCount).ThenByDescending(result => result.CommunityRating);
    }

    private async Task<MediaInfo> GetMediaStreamsAsync(string mediaPath, CancellationToken cancellationToken)
    {
        var requestInfo = new MediaInfoRequest
        {
            MediaSource = new MediaSourceInfo
            {
                Id = Guid.NewGuid().ToString(),
                Path = mediaPath,
                Protocol = MediaProtocol.File,
                Container = System.IO.Path.GetExtension(mediaPath).TrimStart('.')
            },
            ExtractChapters = false,
            MediaType = DlnaProfileType.Video
        };
        return await mediaEncoder.GetMediaInfo(requestInfo, cancellationToken).ConfigureAwait(false);
    }
}
