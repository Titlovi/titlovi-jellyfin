using System.Globalization;
using System.Net;
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
    private readonly HttpClient httpClient;

    public TitloviSubtitleProvider(ILogger<TitloviSubtitleProvider> logger, ITitloviManager titloviManager, IMediaEncoder mediaEncoder, IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        this.titloviManager = titloviManager;
        this.mediaEncoder = mediaEncoder;
        this.httpClient = httpClientFactory.CreateClient("HttpTitloviClient");
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

        var response = await httpClient.GetAsync($"download/?mediaid={idParts[0]}&type={idParts[1]}", cancellationToken).ConfigureAwait(false);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new HttpRequestException($"Failed to download subtitle with id: {id}");
        }

        return new SubtitleResponse()
        {
            Format = "srt",
            IsForced = false,
            IsHearingImpaired = false,
            Language = idParts[2].FromProviderLanguage(),
            Stream = new MemoryStream(await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false)),
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
            Lang = request.Language.ToProviderLanguage(),
            IgnoreLangAndEpisode = false,
            Season = request.ParentIndexNumber,
            Episode = request.IndexNumber
        }).ConfigureAwait(false);

        if (response is null)
        {
            return new List<RemoteSubtitleInfo>();
        }

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
            Comment = string.Empty,
            ThreeLetterISOLanguageName = result.Language.FromProviderLanguage()
        }).OrderByDescending(result => result.DownloadCount);
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
