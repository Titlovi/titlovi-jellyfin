using System.Text.Json.Serialization;

namespace Titlovi.Api.Models;

/// <summary>
/// Represents an authentication token returned by the Titlovi service.
/// </summary>
public sealed class Token
{
    /// <summary>
    /// Gets the token identifier string.
    /// </summary>
    [JsonPropertyName("Token")]
    public required string Id { get; init; }

    /// <summary>
    /// Gets the user's unique identifier.
    /// </summary>
    [JsonPropertyName("UserId")]
    public required int UserId { get; init; }

    /// <summary>
    /// Gets the username associated with this token.
    /// </summary>
    [JsonPropertyName("UserName")]
    public required string UserName { get; init; }

    /// <summary>
    /// Gets the date and time when this token expires.
    /// </summary>
    [JsonPropertyName("ExpirationDate")]
    public required DateTime ExpirationDate { get; init; }

    /// <summary>
    /// Gets a flag that tells if the current token has expired.
    /// </summary>
    public bool IsExpired => DateTime.Now >= ExpirationDate;
}