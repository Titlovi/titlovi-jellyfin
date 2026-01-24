namespace Titlovi.Api.Models.Requests;

/// <summary>
/// Represents login credentials for authentication requests.
/// </summary>
public sealed class LoginRequest
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public required string Password { get; set; }
}