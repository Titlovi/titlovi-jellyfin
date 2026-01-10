namespace Titlovi.Api.Models;

public sealed class LoginRequest
{
  public required string Username { get; set; }

  public required string Password { get; set; }
}
