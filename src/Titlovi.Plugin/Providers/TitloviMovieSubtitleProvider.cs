using MediaBrowser.Controller.MediaEncoding;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using System.Collections.Immutable;
using System.Text.Json;
using MediaBrowser.Common.Extensions;
using Titlovi.Api;
using Titlovi.Api.Models;
using Titlovi.Plugin.Extensions;

namespace Titlovi.Plugin.Providers;

/// <summary>
/// Movie Subtitle provider for movies from Titlovi.com.
/// </summary>
public sealed class TitloviMovieSubtitleProvider(
    IMediaEncoder mediaEncoder,
    IKodiClient kodiClient,
    ITitloviClient titloviClient
) : TitloviSubtitleProvider("Titlovi.com - Movies", VideoContentType.Movie)
{
    /// <inheritdoc />
    public override async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        var targetSubtitle = JsonSerializer.Deserialize<SubtitleMetadata>(Convert.FromBase64String(id));
        if (targetSubtitle == null)
            throw new ResourceNotFoundException("Failed to deserialize internal subtitle download request");

        var response = await titloviClient.DownloadSubtitle(targetSubtitle.ToDownloadRequest()).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
            throw new ResourceNotFoundException($"Failed to download subtitle [type={targetSubtitle.Type}, mediaId={targetSubtitle.Id}, code={response.StatusCode}]");

        ArgumentNullException.ThrowIfNull(response.Content);

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        var stream = new MemoryStream(bytes);

        var subtitles = ExtractSubtitles(stream);
        return subtitles.Count == 0 ? throw new ResourceNotFoundException($"Compressed Subtitle file contained no valid subtitles [type={targetSubtitle.Type}, mediaId={targetSubtitle.Id}]") : subtitles.First().ToResponse(targetSubtitle.Language.FromProviderLanguage());
    }

    /// <inheritdoc />
    public override async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        if (request.DisabledSubtitleFetchers.Contains(Name))
            return [];

        var imdbId = request.ProviderIds?.GetValueOrDefault("Imdb");
        var token = await GetTokenAsync(kodiClient).ConfigureAwait(false);
        var subtitles = new List<Subtitle>();

        await CollectSubtitles(kodiClient, subtitles, request, token, 1, imdbId, null).ConfigureAwait(false);

        var mediaInfo = await GetMediaInfoAsync(mediaEncoder, request.MediaPath, cancellationToken).ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(mediaInfo);

        return [.. subtitles
            .OrderByDescending(mediaInfo.HashScore)
            .ThenByDescending(subtitle => subtitle.DownloadCount)
            .ThenByDescending(subtitle => subtitle.Rating)
            .Select(result => result.ToRemoteSubtitleInfo(Name))
        ];
    }
}