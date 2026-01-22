using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Controller.Subtitles;
using Microsoft.Extensions.DependencyInjection;
using Titlovi.Api.Extensions;
using Titlovi.Plugin.Providers;

namespace Titlovi.Plugin;

public sealed class TitloviServiceRegistrator : IPluginServiceRegistrator
{
    // <inheritdoc />
    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        serviceCollection.AddTitloviApi();
        serviceCollection.AddSingleton<ISubtitleProvider, TitloviSubtitleProvider>();
    }
}