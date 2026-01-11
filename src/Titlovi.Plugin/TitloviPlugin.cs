using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Titlovi.Plugin.Configuration;

namespace Titlovi.Plugin;

public sealed class TitloviPlugin : BasePlugin<TitloviConfiguration>, IHasWebPages
{
    public TitloviPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
      : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <summary>
    /// Gets the current plugin instance.
    /// </summary>
    public static TitloviPlugin? Instance { get; private set; }

    /// <inheritdoc />
    public override string Name => "Titlovi.com";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("6e25df50-638e-4109-a50b-03c14fc93fdd");

    // <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages() => [
      new()
    {
      Name = Name,
      EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.TitloviConfiguration.html"
    },
    new()
    {
      Name = $"{Name}-js",
      EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.TitloviConfiguration.js"
    }
    ];
}