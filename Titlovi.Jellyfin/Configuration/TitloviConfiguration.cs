using MediaBrowser.Model.Plugins;

namespace Titlovi.Jellyfin.Configuration;

/// <summary>
/// Languages: "Bosanski", "Cirilica", "English", "Hrvatski", "Makedonski", "Slovenski", "Srpski".
/// </summary>
public class TitloviConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets a username used for authenticating
    /// with the Titlovi api endpoint.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a password used for authenticating
    /// with the Titlovi api endpoint.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
