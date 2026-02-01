using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using System.Collections.Immutable;
using Titlovi.Api;
using Titlovi.Api.Models.Enums;
using Titlovi.Plugin.Extensions;

namespace Titlovi.Plugin.Providers;

/// <summary>
/// Movie Subtitle provider for movies from Titlovi.com.
/// </summary>
public sealed class TitloviMovieSubtitleProvider(IKodiClient kodiClient, ITitloviClient titloviClient) : TitloviSubtitleProvider("Titlovi.com - Movies", VideoContentType.Movie)
{
    /// <summary>
    /// Gets an empty <seealso cref="SubtitleResponse"/>.
    /// </summary>
    private static readonly SubtitleResponse EmptySubtitle = new();

    /// <summary>
    /// Downloads subtitles for the specified movie ID.
    /// </summary>
    /// <param name="id">Subtitle identifier in format "mediaId:language".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Subtitle response containing the downloaded subtitle file, or empty response if download fails.</returns>
    public override async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        var metadata = id.Split(':');
        if (metadata.Length < 2 || !int.TryParse(metadata[0], out var mediaId))
            return EmptySubtitle;

        var response = await titloviClient.DownloadSubtitle(new(mediaId, SubtitleType.Movie)).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
            return EmptySubtitle;

        ArgumentNullException.ThrowIfNull(response.Content);

        var subtitles = ExtractSubtitles(await response.Content.ReadAsMemoryStream().ConfigureAwait(false));
        if (subtitles.Count == 0)
            return EmptySubtitle;

        return new()
        {
            Format = "srt",
            IsForced = false,
            IsHearingImpaired = false,
            Language = metadata[1],
            Stream = subtitles.First().Buffer,
        };
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

        return [.. (await kodiClient.Search(new()
        {
            Token = token.Id,
            UserId = token.UserId,
            Type = SubtitleType.Movie,
            ImdbId = imdbId,
            Query = imdbId ?? request.Name,
            Lang = request.Language.ToProviderLanguage(),
            IgnoreLangAndEpisode = false,
        }).ConfigureAwait(false)).Results.Select(result => result.ToRemoteSubtitleInfo(Name))];
    }
}