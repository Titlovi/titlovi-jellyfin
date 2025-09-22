using System.Globalization;
using Jellyfin.Data.Entities.Libraries;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using Titlovi.Jellyfin.Interfaces;
using Titlovi.Jellyfin.Models;
using Titlovi.Jellyfin.Models.Subtitle;

namespace Titlovi.Jellyfin.Providers;

/// <summary>
/// Actual provider that gives jellyfin
/// access to subtitles from titlovi.com.
/// </summary>
public class TitloviSubtitlesProvider : ISubtitleProvider
{
    private readonly ILogger<TitloviSubtitlesProvider> logger;
    private readonly ITitloviManager titloviManager;

    public TitloviSubtitlesProvider(ILogger<TitloviSubtitlesProvider> logger, ITitloviManager titloviManager)
    {
        this.logger = logger;
        this.titloviManager = titloviManager;
    }

    /// <inheritdoc />
    public string Name => "Titlovi.com";

    /// <inheritdoc />
    public IEnumerable<VideoContentType> SupportedMediaTypes => new[]
    {
      VideoContentType.Episode,
      VideoContentType.Movie
    };

    /// <inheritdoc />
    public async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        return new SubtitleResponse();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        var imdbId = request.ProviderIds?.GetValueOrDefault("Imdb");
        if (string.IsNullOrEmpty(imdbId))
        {
            return [];
        }

        var response = await titloviManager.SearchAsync(
            new LoginInfo() { Username = TitloviJellyfin.Instance?.Configuration.Username!, Password = TitloviJellyfin.Instance?.Configuration.Password! },
            new SubtitleSearch() { ImdbId = imdbId }
        ).ConfigureAwait(false);

        return response.Results.Select(result =>
        {
            return new RemoteSubtitleInfo()
            {
                Id = Convert.ToString(result.Id, CultureInfo.InvariantCulture),
                ProviderName = Name,
                AiTranslated = false,
                Name = result.Title,
                CommunityRating = result.Rating,
                DownloadCount = result.DownloadCount,
            };
        }).ToList().OrderBy(result => result.DownloadCount).Reverse();
    }
}
