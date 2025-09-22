using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Titlovi.Jellyfin;

/// <summary>
/// Used for registering services. 
/// </summary>
public class TitloviServiceProvider : IPluginServiceRegistrator
{
    /// <inheritdoc />
    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        throw new NotImplementedException();
    }
}
