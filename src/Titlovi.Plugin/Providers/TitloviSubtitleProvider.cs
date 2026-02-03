using MediaBrowser.Controller.MediaEncoding;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Providers;
using System.IO.Compression;
using Titlovi.Api;
using Titlovi.Api.Models;
using Titlovi.Api.Models.Enums;
using Titlovi.Api.Models.Requests;
using Titlovi.Plugin.Extensions;

namespace Titlovi.Plugin.Providers;

/// <summary>
/// Base class for Titlovi subtitle providers supporting multiple video content types.
/// </summary>
public abstract class TitloviSubtitleProvider(string name, params VideoContentType[] contentTypes) : ISubtitleProvider
{
    /// <summary>
    /// Gets an empty <seealso cref="SubtitleResponse"/>.
    /// </summary>
    protected static readonly SubtitleResponse EmptySubtitle = new();

    /// <summary>
    /// Gets the name of the <seealso cref="ISubtitleProvider"/>.
    /// </summary>
    public string Name => name;

    /// <summary>
    /// Gets the types of video content this <seealso cref="ISubtitleProvider"/> is used for.
    /// </summary>
    public IEnumerable<VideoContentType> SupportedMediaTypes => [.. contentTypes];

    /// <summary>
    /// Downloads subtitles for the specified ID.
    /// </summary>
    /// <param name="id">Subtitle identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Subtitle response containing the downloaded subtitle files.</returns>
    public abstract Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Searches for available subtitles matching the request criteria.
    /// </summary>
    /// <param name="request">Search criteria including title, year, and language.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of matching subtitle entries.</returns>
    public abstract Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Gets authentication token from plugin configuration, refreshing if expired.
    /// </summary>
    /// <param name="kodiClient">Kodi client instance for token retrieval.</param>
    /// <returns>Valid authentication token.</returns>
    protected static async Task<Token> GetTokenAsync(IKodiClient kodiClient)
    {
        ArgumentNullException.ThrowIfNull(kodiClient);

        var pluginInstance = TitloviPlugin.Instance;
        var pluginConfiguration = pluginInstance?.Configuration!;

        var token = pluginConfiguration.Token;
        if (token == null || token.IsExpired)
        {
            token = await kodiClient.GetToken(pluginConfiguration.Username, pluginConfiguration.Password).ConfigureAwait(false);

            pluginConfiguration.Token = token;
            pluginInstance?.SaveConfiguration();
        }

        return token;
    }

    /// <summary>
    /// Extracts .srt subtitle files from a ZIP archive or returns the buffer as-is if extraction fails.
    /// </summary>
    /// <param name="buffer">Memory stream containing ZIP archive or raw subtitle data.</param>
    /// <returns>Collection of extracted subtitle files.</returns>
    public static ICollection<SubtitleFile> ExtractSubtitles(MemoryStream buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        try
        {
            using var archive = new ZipArchive(buffer, ZipArchiveMode.Read);
            ICollection<SubtitleFile> subtitles = [.. archive.Entries
                .Where(entry => entry.Name.EndsWith(".srt", StringComparison.OrdinalIgnoreCase))
                .Select(entry =>
                {
                    using var entryStream = entry.Open();
                    var extractedStream = new MemoryStream();

                    entryStream.CopyTo(extractedStream);
                    extractedStream.Position = 0;

                    return new SubtitleFile(entry.Name, extractedStream);
                })];

            buffer.Close();
            return subtitles;
        }
        catch (Exception)
        {
            buffer.Position = 0;
            return [new(string.Empty, buffer)];
        }
    }

    /// <summary>
    /// Retrieves detailed media information including stream properties for the specified media file.
    /// </summary>
    /// <param name="mediaEncoder">Instance of the <seealso cref="IMediaEncoder"/></param>
    /// <param name="mediaPath">Full path to the media file to analyze.</param>
    /// <param name="cancellationToken">Token to cancel the media analysis operation.</param>
    /// <returns>Media information including codec, resolution, and stream details.</returns>
    /// <remarks>
    /// Currently unused but intended for future enhancement to match subtitles based on
    /// media properties like resolution, codec, and release group.
    /// </remarks>
    protected static async Task<MediaInfo> GetMediaInfoAsync(IMediaEncoder mediaEncoder, string mediaPath, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(mediaEncoder);

        var requestInfo = new MediaInfoRequest
        {
            MediaSource = new MediaSourceInfo
            {
                Id = Guid.NewGuid().ToString(),
                Path = mediaPath,
                Protocol = MediaProtocol.File,
                Container = Path.GetExtension(mediaPath).TrimStart('.')
            },
            ExtractChapters = false,
            MediaType = DlnaProfileType.Video
        };

        return await mediaEncoder.GetMediaInfo(requestInfo, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Recursively collects all subtitles from paginated search results.
    /// </summary>
    /// <param name="kodiClient">Kodi client instance for API requests.</param>
    /// <param name="subtitles">Collection to populate with found subtitles.</param>
    /// <param name="request">Search criteria for subtitles.</param>
    /// <param name="token">Authentication token.</param>
    /// <param name="page">Current page number to retrieve.</param>
    /// <param name="imdbId">Optional IMDb identifier for the content.</param>
    /// <param name="index">Optional episode index for series content.</param>
    protected static async Task CollectSubtitles(IKodiClient kodiClient, IList<Subtitle> subtitles, SubtitleSearchRequest request, Token token, int page, string? imdbId = null, int? index = null)
    {
        ArgumentNullException.ThrowIfNull(kodiClient);
        ArgumentNullException.ThrowIfNull(subtitles);

        var response = await kodiClient.Search(CreateSearchRequest(token, request, page, imdbId, index)).ConfigureAwait(false);
        foreach (var subtitle in response.Results)
            subtitles.Add(subtitle);

        if (response.CurrentPage < response.PagesAvailable)
            await CollectSubtitles(kodiClient, subtitles, request, token, page + 1, imdbId, index).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a search request for the Kodi API from subtitle search criteria.
    /// </summary>
    /// <param name="token">Authentication token.</param>
    /// <param name="request">Search criteria for subtitles.</param>
    /// <param name="page">Page number to retrieve.</param>
    /// <param name="imdbId">Optional IMDb identifier for the content.</param>
    /// <param name="index">Optional episode index for series content.</param>
    /// <returns>Configured search request for the API.</returns>
    protected static SearchSubtitleRequest CreateSearchRequest(Token token, SubtitleSearchRequest request, int page, string? imdbId = null, int? index = null)
    {
        ArgumentNullException.ThrowIfNull(token);
        ArgumentNullException.ThrowIfNull(request);

        return new()
        {
            Token = token.Id,
            UserId = token.UserId,
            Type = index == null ? SubtitleType.Movie : SubtitleType.Episode,
            Query = imdbId ?? request.SeriesName,
            Lang = request.Language.ToProviderLanguage(),
            IgnoreLangAndEpisode = false,
            Season = request.ParentIndexNumber,
            Episode = index,
            Page = page,
            ImdbId = imdbId,
        };
    }
}