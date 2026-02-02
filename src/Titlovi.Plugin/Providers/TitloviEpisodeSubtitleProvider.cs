using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using System.Text.Json;
using System.Text.RegularExpressions;
using Titlovi.Api;
using Titlovi.Api.Models;
using Titlovi.Api.Models.Enums;
using Titlovi.Api.Models.Requests;
using Titlovi.Plugin.Extensions;

namespace Titlovi.Plugin.Providers;

public sealed partial class TitloviEpisodeSubtitleProvider(
    IKodiClient kodiClient,
    ITitloviClient titloviClient
) : TitloviSubtitleProvider("Titlovi.com - Episodes", VideoContentType.Episode)
{
    [GeneratedRegex(@"[sS](?<season>\d+)\.?[eE](?<episode>\d+)", RegexOptions.Compiled)]
    private static partial Regex EpisodeRegex();

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

    public override async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        if (request.DisabledSubtitleFetchers.Contains(Name))
            return [];

        var token = await GetTokenAsync(kodiClient).ConfigureAwait(false);
        var subtitles = new List<Subtitle>();

        await CollectSubtitles(subtitles, request, token, 0, 1).ConfigureAwait(false);
        await CollectSubtitles(subtitles, request, token, request.IndexNumber.GetValueOrDefault(), 1).ConfigureAwait(false);

        subtitles.ForEach(subtitle => subtitle.Episode = request.IndexNumber.GetValueOrDefault());
        return [.. subtitles.Select(result => result.ToRemoteSubtitleInfo(Name)).ToList()];
    }

    private async Task CollectSubtitles(List<Subtitle> subtitles, SubtitleSearchRequest request, Token token, int episode, int page)
    {
        var response = await kodiClient.Search(CreateSearchRequest(token, request, page, episode)).ConfigureAwait(false);
        subtitles.AddRange(response.Results);

        if (response.CurrentPage < response.PagesAvailable)
            await CollectSubtitles(subtitles, request, token, episode, page + 1).ConfigureAwait(false);
    }

    private static SearchSubtitleRequest CreateSearchRequest(Token token, SubtitleSearchRequest request, int page, int? episode = null) => new()
    {
        Token = token.Id,
        UserId = token.UserId,
        Type = SubtitleType.Episode,
        Query = request.SeriesName,
        Lang = request.Language.ToProviderLanguage(),
        IgnoreLangAndEpisode = false,
        Season = request.ParentIndexNumber,
        Episode = episode,
        Page = page
    };
}
