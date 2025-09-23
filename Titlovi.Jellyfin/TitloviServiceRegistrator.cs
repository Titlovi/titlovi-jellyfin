using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Controller.Subtitles;
using Microsoft.Extensions.DependencyInjection;
using Titlovi.Jellyfin.Interfaces;
using Titlovi.Jellyfin.Providers;

namespace Titlovi.Jellyfin;

/// <summary>
/// Used for registering services.
/// </summary>
public class TitloviServiceRegistrator : IPluginServiceRegistrator
{
    /// <inheritdoc />
    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        serviceCollection.AddSingleton<ITitloviManager, TitloviManager>();
        serviceCollection.AddSingleton<ISubtitleProvider, TitloviSubtitleProvider>();

        serviceCollection.AddHttpClient("HttpKodiClient", httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://kodi.titlovi.com/api/subtitles/");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");
        });
        serviceCollection.AddHttpClient("HttpTitloviClient", httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://titlovi.com/");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");
        });
    }
}
