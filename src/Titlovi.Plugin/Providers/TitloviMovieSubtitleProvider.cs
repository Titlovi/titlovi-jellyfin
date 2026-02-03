using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using System.Collections.Immutable;
using System.Text.Json;
using Titlovi.Api;
using Titlovi.Api.Models;
using Titlovi.Api.Models.Enums;
using Titlovi.Plugin.Extensions;

namespace Titlovi.Plugin.Providers;

/// <summary>
/// Movie Subtitle provider for movies from Titlovi.com.
/// </summary>
public sealed class TitloviMovieSubtitleProvider(IKodiClient kodiClient, ITitloviClient titloviClient) : TitloviSubtitleProvider("Titlovi.com - Movies", VideoContentType.Movie)
{
    /// <summary>
    /// Downloads subtitles for the specified movie ID.
    /// </summary>
    /// <param name="id">Subtitle identifier in format "mediaId:language".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Subtitle response containing the downloaded subtitle file, or empty response if download fails.</returns>
    public override async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        var targetSubitle = JsonSerializer.Deserialize<SubtitleMetadata>(Convert.FromBase64String(id));
        if (targetSubitle == null)
            return EmptySubtitle;

        var response = await titloviClient.DownloadSubtitle(targetSubitle.ToDownloadRequest()).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
            return EmptySubtitle;

        ArgumentNullException.ThrowIfNull(response.Content);

        var subtitles = ExtractSubtitles(await response.Content.ReadAsMemoryStream().ConfigureAwait(false));
        if (subtitles.Count == 0)
            return EmptySubtitle;

        return subtitles.First().ToResponse(targetSubitle.Language.FromProviderLanguage());
    }

    /// <summary>
    /// Searches for available movie subtitles matching the request criteria.
    /// </summary>
    /// <param name="request">Search criteria including movie title, IMDB ID, and language.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of matching movie subtitle entries.</returns>
    public override async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        if (request.DisabledSubtitleFetchers.Contains(Name))
            return [];

        var imdbId = request.ProviderIds?.GetValueOrDefault("Imdb");
        var token = await GetTokenAsync(kodiClient).ConfigureAwait(false);
        var subtitles = new List<Subtitle>();

        await CollectSubtitles(kodiClient, subtitles, request, token, 1, imdbId, null).ConfigureAwait(false);
        await CollectSubtitles(kodiClient, subtitles, request, token, 1, imdbId, null).ConfigureAwait(false);

        return [.. subtitles.Select(result => result.ToRemoteSubtitleInfo(Name))];
    }
}