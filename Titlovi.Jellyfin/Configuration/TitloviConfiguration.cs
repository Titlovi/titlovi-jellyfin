using MediaBrowser.Model.Plugins;
using Titlovi.Jellyfin.Models;

namespace Titlovi.Jellyfin.Configuration;

/// <summary>
/// Languages: "Bosanski", "Cirilica", "English", "Hrvatski", "Makedonski", "Slovenski", "Srpski".
/// </summary>
public class TitloviConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets a login information
    /// used for authenticating with the Titlovi api endpoint.
    /// </summary>
    public LoginInfo LoginInfo { get; set; } = new LoginInfo()
    {
        Username = string.Empty,
        Password = string.Empty
    };

    /// <summary>
    /// Gets or sets a token information
    /// used for communication with the Titlovi api endpoint.
    /// </summary>
    public TokenInfo? TokenInfo { get; set; } = null;
}
