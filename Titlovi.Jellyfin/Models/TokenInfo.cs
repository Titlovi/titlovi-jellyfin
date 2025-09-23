using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models;

public class TokenInfo
{
    /// <summary>
    /// Gets or sets the expiration date.
    /// </summary>
    [JsonPropertyName("ExpirationDate")]
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    [JsonPropertyName("Token")]
    public Guid Token { get; set; }

    /// <summary>
    /// Gets or sets the user id.
    /// </summary>
    [JsonPropertyName("UserId")]
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [JsonPropertyName("UserName")]
    public string Username { get; set; } = string.Empty;
}
