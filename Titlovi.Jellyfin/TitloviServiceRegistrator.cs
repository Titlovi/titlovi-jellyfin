using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Titlovi.Jellyfin.Interfaces;
using Titlovi.Jellyfin.Services;

namespace Titlovi.Jellyfin;

/// <summary>
/// Used for registering services. 
/// </summary>
public class TitloviServiceProvider : IPluginServiceRegistrator
{
    /// <inheritdoc />
    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        serviceCollection.AddSingleton<ITitloviManager, TitloviManager>();
    }
}
