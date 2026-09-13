using MediaBrowser.Controller.MediaEncoding;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.RegularExpressions;
using MediaBrowser.Common.Extensions;
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
    ITitloviClient titloviClient,
    ILogger<TitloviEpisodeSubtitleProvider> logger
) : TitloviSubtitleProvider("Titlovi.com - Episodes", VideoContentType.Episode)
{
    [GeneratedRegex(@"(?:S|Season)[\-. ]?(?<season>\d{1,2})[\-. ]?(?:E|Ę|Episode)[\-. ]?(?<episode>\d{1,2})|(?<season>\d{1,2})x(?<episode>\d{1,2})|E(?<episode>\d{1,2})|Part[\-. ]?(?<episode>\d{1,2})|^(?<episode>\d{1,2})\D|\D(?<episode>\d{1,2})$", RegexOptions.IgnoreCase)]
    private static partial Regex EpisodeRegEx();


    private static Match? GetBestEpisodeMatch(string input)
    {
        MatchCollection matches = EpisodeRegEx().Matches(input);

        return matches
            .FirstOrDefault(match =>
                match.Groups["season"].Success &&
                match.Groups["episode"].Success)
            ?? matches
                .OrderByDescending(match => match.Value.Length)
                .FirstOrDefault();
    }

    private static bool IsMatchingEpisode(
        string fileName,
        int season,
        int episode)
    {
        var match = GetBestEpisodeMatch(
            Path.GetFileNameWithoutExtension(fileName));

        if (match is null)
            return false;

        if (!int.TryParse(
                match.Groups["episode"].Value,
                out var matchedEpisode)
            || matchedEpisode != episode)
        {
            return false;
        }

        // Ako filename sadrži i sezonu, mora odgovarati i sezona.
        if (match.Groups["season"].Success)
        {
            return int.TryParse(
                    match.Groups["season"].Value,
                    out var matchedSeason)
                && matchedSeason == season;
        }

        // E03, Part03, "03 - naziv" i slično nemaju sezonu,
        // pa uspoređujemo samo epizodu.
        return true;
    }

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
        if (subtitles.Count == 0)
            throw new ResourceNotFoundException($"Compressed Subtitle file contained no available subtitles [type={targetSubtitle.Type}, mediaId={targetSubtitle.Id}]");

        foreach (var subtitle in subtitles)
        {
            if (IsMatchingEpisode(
                    subtitle.Path,
                    targetSubtitle.Season,
                    targetSubtitle.Episode))
            {
                return subtitle.ToResponse(
                    targetSubtitle.Language.FromProviderLanguage());
            }
        }

        var subtitlePaths = string.Join(',', subtitles.Select(subtitle => subtitle.Path).ToList());
        logger.LogWarning(
    "S={Season}, E={Episode}, not found in: {SubtitlePaths}",
    targetSubtitle.Season,
    targetSubtitle.Episode,
    subtitlePaths);

        throw new ResourceNotFoundException($"Failed to locate matching subtitle for target season and episode [type={targetSubtitle.Type}, mediaId={targetSubtitle.Id}, season={targetSubtitle.Season}, episode={targetSubtitle.Episode}]");
    }

    /// <inheritdoc />
    public override async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        if (request.DisabledSubtitleFetchers.Contains(Name))
            return [];

        var token = await GetTokenAsync(kodiClient).ConfigureAwait(false);
        var subtitles = new List<Subtitle>();

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
