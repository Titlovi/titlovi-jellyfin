using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models;

/// <summary>
/// Contains authentication token information received from the Titlovi.com API after successful login.
/// </summary>
/// <remarks>
/// This class represents the response from the authentication endpoint and is used for subsequent
/// API calls that require authentication. Tokens have an expiration date and should be refreshed
/// when they expire.
/// </remarks>
public class TokenInfo
{
    /// <summary>
    /// Gets or sets the date and time when the authentication token expires.
    /// </summary>
    /// <value>
    /// The expiration timestamp for the token. After this time, a new token must be obtained
    /// through re-authentication.
    /// </value>
    [JsonPropertyName("ExpirationDate")]
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the unique authentication token.
    /// </summary>
    /// <value>
    /// A GUID representing the session token used to authenticate API requests.
    /// This token must be included in all authenticated API calls.
    /// </value>
    [JsonPropertyName("Token")]
    public Guid Token { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the authenticated user.
    /// </summary>
    /// <value>
    /// The numeric user ID assigned by Titlovi.com for the authenticated account.
    /// Used in API requests that require user identification.
    /// </value>
    [JsonPropertyName("UserId")]
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username of the authenticated user.
    /// </summary>
    /// <value>
    /// The username returned by the authentication service, typically matching the
    /// username from the login request. Defaults to an empty string.
    /// </value>
    [JsonPropertyName("UserName")]
    public string Username { get; set; } = string.Empty;
}
