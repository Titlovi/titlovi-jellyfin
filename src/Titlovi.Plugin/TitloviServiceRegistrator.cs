using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Titlovi.Api.Extensions;
using Titlovi.Plugin.Extensions;

namespace Titlovi.Plugin;

public sealed class TitloviServiceRegistrator : IPluginServiceRegistrator
{
    // <inheritdoc />
    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        serviceCollection.AddTitloviApi();
        serviceCollection.AddTitloviSubtitleProviders();
    }
}