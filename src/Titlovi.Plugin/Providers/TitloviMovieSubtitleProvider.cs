using MediaBrowser.Controller.Authentication;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using Titlovi.Api;
using Titlovi.Api.Models.Enums;
using Titlovi.Plugin.Extensions;

namespace Titlovi.Plugin.Providers;

public sealed class TitloviMovieSubtitleProvider(IKodiClient kodiClient, ITitloviClient titloviClient) : TitloviSubtitleProvider("Titlovi.com - Movies", VideoContentType.Movie)
{
    public override async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        var metadata = id.Split(':');
        if (metadata.Length < 2 || !int.TryParse(metadata[0], out var mediaId))
            return new();

        var response = await titloviClient.DownloadSubtitle(new()
        {
            MediaId = mediaId,
            Type = SubtitleType.Movie
        }).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return new();

        return new()
        {
            Format = "srt",
            IsForced = false,
            IsHearingImpaired = false,
            Language = metadata[1],
            Stream = response.Content,
        };
    }

    public override async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        var pluginInstance = TitloviPlugin.Instance;
        if (pluginInstance is null || request.DisabledSubtitleFetchers.Contains(Name))
            return [];

        var configuration = pluginInstance.Configuration;
        var token = configuration.Token;

        if (token is null || token.ExpirationDate <= DateTime.Now)
        {
            try
            {
                token = await kodiClient.GetToken(configuration.Username, configuration.Password).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw new AuthenticationException("You titlovi.com credentials are not invalid");
            }
        }

        var imdbId = request.ProviderIds?.GetValueOrDefault("Imdb");
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