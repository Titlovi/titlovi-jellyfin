using MediaBrowser.Model.Plugins;
using Titlovi.Jellyfin.Models;

namespace Titlovi.Jellyfin.Configuration;

/// <summary>
/// Configuration settings for the Titlovi.Jellyfin subtitle provider plugin.
/// </summary>
public class TitloviConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the login credentials used for authenticating with the Titlovi API endpoint.
    /// </summary>
    /// <value>
    /// Login information containing username and password. Defaults to empty credentials that must be configured by the user.
    /// </value>
    public LoginInfo LoginInfo { get; set; } = new LoginInfo()
    {
        Username = string.Empty,
        Password = string.Empty
    };

    /// <summary>
    /// Gets or sets the authentication token used for API communication with the Titlovi service.
    /// </summary>
    /// <value>
    /// Token information for authenticated requests, or null if no token has been obtained yet.
    /// Typically populated after successful login authentication.
    /// </value>
    public TokenInfo? TokenInfo { get; set; } = null;
}
