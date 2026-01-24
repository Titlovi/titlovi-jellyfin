using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Search;
using Titlovi.Api;
using Titlovi.Api.Models.Enums;

namespace Titlovi.Plugin.Providers;

public sealed class TitloviMovieSubtitleProvider(IKodiClient kodiClient) : TitloviSubtitleProvider("Titlovi.com - Movies", VideoContentType.Movie)
{
    public override Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        var pluginInstance = TitloviPlugin.Instance;
        if (pluginInstance is null || request.DisabledSubtitleFetchers.Contains(Name))
            return [];

        var token = pluginInstance.Configuration.Token;
        if (token is null || token.ExpirationDate <= DateTime.Now)
            return []; // TODO: alert the user that their authentication token has expired.

        var imdb = request.ProviderIds?.GetValueOrDefault("Imdb");
        return (await kodiClient.Search(new()
        {
            Token = token.Id,
            UserId = token.UserId,
            Type = SubtitleType.Movie,
            ImdbId = imdb,
            Query = imdb == null ? request.Name : null,
            IgnoreLangAndEpisode = true,
        }).ConfigureAwait(false)).Results.Select(result => new RemoteSubtitleInfo()
        {
            Id = $"{result.Id}-{result.Type}-{result.Language}-{request.IndexNumber}",
            Name = result.Title,
            CommunityRating = result.Rating,
            DownloadCount = result.DownloadCount,
            ProviderName = Name,
            AiTranslated = false,
            DateCreated = result.Date,
            Format = "srt",
            Author = string.Empty,
            Comment = result.Release
        }).ToList();
    }
}