using System.Globalization;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Titlovi.Jellyfin.Configuration;

namespace Titlovi.Jellyfin;

/// <summary>
/// Main plugin class for the Titlovi.com subtitle provider integration with Jellyfin.
/// </summary>
/// <remarks>
/// This plugin enables Jellyfin to search for and download subtitles from Titlovi.com,
/// a popular subtitle service for Balkan languages. Provides a web-based configuration
/// interface for setting up user credentials.
/// </remarks>
public class TitloviJellyfin : BasePlugin<TitloviConfiguration>, IHasWebPages
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TitloviJellyfin"/> class.
    /// </summary>
    /// <param name="applicationPaths">Jellyfin application paths for configuration storage.</param>
    /// <param name="xmlSerializer">XML serializer for configuration persistence.</param>
    public TitloviJellyfin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <inheritdoc />
    public override string Name => "Titlovi.com";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("6e25df50-638e-4109-a50b-03c14fc93fdd");

    /// <summary>
    /// Gets the current plugin instance for global access.
    /// </summary>
    /// <value>
    /// The singleton instance of the plugin, or null if not yet initialized.
    /// Used by other plugin components to access configuration and services.
    /// </value>
    public static TitloviJellyfin? Instance { get; private set; }

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        return
        [
            new PluginPageInfo
            {
                Name = Name,
                EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.TitloviConfiguration.html", GetType().Namespace)
            }
        ];
    }
}
