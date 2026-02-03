using MediaBrowser.Model.Plugins;
using Titlovi.Api.Models;

namespace Titlovi.Plugin.Configuration;

/// <summary>
/// Configuration for the Titlovi plugin containing authentication credentials and token.
/// </summary>
public sealed class TitloviConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the Titlovi username for authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Titlovi password for authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the cached authentication token to avoid repeated logins.
    /// </summary>
    public Token? Token { get; set; }
}