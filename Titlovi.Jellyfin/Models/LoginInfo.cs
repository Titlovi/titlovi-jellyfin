using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models;

/// <summary>
/// Contains user credentials required for authenticating with the Titlovi.com API.
/// </summary>
/// <remarks>
/// This class is serialized to JSON when sending login requests to the Titlovi.com service.
/// Both username and password are required for successful authentication.
/// </remarks>
public class LoginInfo
{
    /// <summary>
    /// Gets or sets the Titlovi.com account username.
    /// </summary>
    /// <value>
    /// The username for the Titlovi.com account. Must be a valid registered username.
    /// Defaults to an empty string and must be configured by the user.
    /// </value>
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Titlovi.com account password.
    /// </summary>
    /// <value>
    /// The password for the Titlovi.com account. Stored in plain text in configuration.
    /// Defaults to an empty string and must be configured by the user.
    /// </value>
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
