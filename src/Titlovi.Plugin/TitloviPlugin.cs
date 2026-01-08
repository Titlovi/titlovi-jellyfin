using System.Globalization;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Titlovi.Plugin.Configuration;

namespace Titlovi.Plugin;

public sealed class TitloviPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
  : BasePlugin<TitloviConfiguration>(applicationPaths, xmlSerializer), IHasWebPages
{
  /// <inheritdoc />
  public override string Name => "Titlovi.com";

  /// <inheritdoc />
  public override Guid Id => Guid.Parse("6e25df50-638e-4109-a50b-03c14fc93fdd");

  // <inheritdoc />
  public IEnumerable<PluginPageInfo> GetPages() => [
    new()
    {
      Name = Name,
      EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.TitloviConfiguration.html", GetType().Namespace)
    }
  ];
}
