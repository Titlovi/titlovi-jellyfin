using MediaBrowser.Controller.MediaEncoding;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using System.Text.Json;
using System.Text.RegularExpressions;
using Titlovi.Api;
using Titlovi.Api.Models;
using Titlovi.Plugin.Extensions;

namespace Titlovi.Plugin.Providers;

/// <summary>
/// Episode Subtitle provider for movies from Titlovi.com.
/// </summary>
public sealed partial class TitloviEpisodeSubtitleProvider(
    IMediaEncoder mediaEncoder,
    IKodiClient kodiClient,
    ITitloviClient titloviClient
) : TitloviSubtitleProvider("Titlovi.com - Episodes", VideoContentType.Episode)
{
    [GeneratedRegex(@"[sS](?<season>\d+)\.?[eE](?<episode>\d+)", RegexOptions.Compiled)]
    private static partial Regex EpisodeRegex();

    /// <inheritdoc />
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

        foreach (var subtitle in subtitles)
        {
            var match = EpisodeRegex().Match(subtitle.Path);
            if (!match.Success)
                continue;

            if (!int.TryParse(match.Groups["season"].Value, out var season))
                continue;
            if (!int.TryParse(match.Groups["episode"].Value, out var episode))
                continue;

            if (targetSubitle.Season == season && targetSubitle.Episode == episode)
                return subtitle.ToResponse(targetSubitle.Language.FromProviderLanguage());
        }

        return EmptySubtitle;
    }

    /// <inheritdoc />
    public override async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        if (request.DisabledSubtitleFetchers.Contains(Name))
            return [];

        var token = await GetTokenAsync(kodiClient).ConfigureAwait(false);
        var subtitles = new List<Subtitle>();

        await CollectSubtitles(kodiClient, subtitles, request, token, 1, null, 0).ConfigureAwait(false);
        await CollectSubtitles(kodiClient, subtitles, request, token, 1, null, request.IndexNumber.GetValueOrDefault()).ConfigureAwait(false);

        var mediaInfo = await GetMediaInfoAsync(mediaEncoder, request.MediaPath, cancellationToken).ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(mediaInfo);

        subtitles.ForEach(subtitle => subtitle.Episode = request.IndexNumber.GetValueOrDefault());
        return [.. subtitles
            .OrderByDescending(mediaInfo.HashScore)
            .ThenByDescending(subtitle => subtitle.DownloadCount)
            .ThenByDescending(subtitle => subtitle.Rating)
            .Select(result => result.ToRemoteSubtitleInfo(Name))
        ];
    }
}
