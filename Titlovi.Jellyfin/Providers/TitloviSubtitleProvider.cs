using System.Collections.ObjectModel;
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
/// Subtitle provider implementation that integrates Jellyfin with the Titlovi.com subtitle service.
/// </summary>
/// <remarks>
/// This provider handles searching for and downloading subtitles from Titlovi.com for movies and TV episodes.
/// Supports multiple Balkan languages and requires user authentication via login credentials.
/// </remarks>
public class TitloviSubtitleProvider : ISubtitleProvider
{
    private readonly ILogger<TitloviSubtitleProvider> logger;
    private readonly ITitloviManager titloviManager;
    private readonly IMediaEncoder mediaEncoder;

    /// <summary>
    /// Initializes a new instance of the <see cref="TitloviSubtitleProvider"/> class.
    /// </summary>
    /// <param name="logger">Logger for recording provider operations and errors.</param>
    /// <param name="titloviManager">Manager for handling Titlovi.com API operations.</param>
    /// <param name="mediaEncoder">Media encoder for analyzing media file properties.</param>
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

    /// <summary>
    /// Downloads and returns subtitle content for the specified subtitle ID.
    /// </summary>
    /// <param name="id">
    /// Composite subtitle identifier in format "MediaId-Type-Language"
    /// (e.g., "12345-1-Hrvatski").
    /// </param>
    /// <param name="cancellationToken">Token to cancel the download operation.</param>
    /// <returns>
    /// A <see cref="SubtitleResponse"/> containing the subtitle stream and metadata.
    /// </returns>
    /// <exception cref="FormatException">Thrown when the subtitle ID format is invalid.</exception>
    /// <exception cref="HttpRequestException">Thrown when the download from Titlovi.com fails.</exception>
    /// <exception cref="InvalidDataException">Thrown when the downloaded data contains no subtitles.</exception>
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

        return new SubtitleResponse()
        {
            Format = "srt",
            IsForced = false,
            IsHearingImpaired = false,
            Language = idParts[2].FromProviderLanguage(), // Convert from provider language to ISO code
            Stream = new MemoryStream(subtitles[0]),
        };
    }

    /// <summary>
    /// Searches for available subtitles matching the given media request.
    /// </summary>
    /// <param name="request">Search criteria including media identifiers and metadata.</param>
    /// <param name="cancellationToken">Token to cancel the search operation.</param>
    /// <returns>
    /// Collection of available subtitle options ordered by download count and rating.
    /// Returns empty collection if no authentication token or IMDB ID is available.
    /// </returns>
    /// <inheritdoc />
    public async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        var tokenInfo = TitloviJellyfin.Instance?.Configuration.TokenInfo;
        var imdbId = request.ProviderIds?.GetValueOrDefault("Imdb");

        if (tokenInfo is null || string.IsNullOrEmpty(imdbId))
        {
            return new List<RemoteSubtitleInfo>();
        }

        var searchQuery = new SubtitleSearch()
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
        };

        var response = await titloviManager.SearchAsync(searchQuery).ConfigureAwait(false);
        if (response is null)
        {
            return new List<RemoteSubtitleInfo>();
        }

        var subtitles = new Collection<Subtitle>(response.Results);
        if (response.PagesAvailable > 1)
        {
            for (int i = response.CurrentPage + 1; i <= response.PagesAvailable; i++)
            {
                searchQuery.Page = i;

                var currentPage = await titloviManager.SearchAsync(searchQuery).ConfigureAwait(false);
                if (currentPage is not null)
                {
                    foreach (var result in currentPage.Results)
                    {
                        subtitles.Add(result);
                    }
                }
            }
        }

        // TODO: Consider media information from GetMediaStreamsAsync (encoder, resolution, etc.)

        return subtitles.Select(result => new RemoteSubtitleInfo()
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
        }).OrderByDescending(result => result.DownloadCount)
          .ThenByDescending(result => result.CommunityRating);
    }

    /// <summary>
    /// Retrieves detailed media information including stream properties for the specified media file.
    /// </summary>
    /// <param name="mediaPath">Full path to the media file to analyze.</param>
    /// <param name="cancellationToken">Token to cancel the media analysis operation.</param>
    /// <returns>Media information including codec, resolution, and stream details.</returns>
    /// <remarks>
    /// Currently unused but intended for future enhancement to match subtitles based on
    /// media properties like resolution, codec, and release group.
    /// </remarks>
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
