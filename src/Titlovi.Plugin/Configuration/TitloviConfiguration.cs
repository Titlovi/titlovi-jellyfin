using MediaBrowser.Model.Plugins;
using Titlovi.Api.Models;

namespace Titlovi.Plugin.Configuration;

public sealed class TitloviConfiguration : BasePluginConfiguration
{
  public string Username { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public Token? Token { get; set; }
}
