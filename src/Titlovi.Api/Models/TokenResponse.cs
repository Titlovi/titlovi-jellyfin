namespace Titlovi.Api.Models;

public sealed class TokenResponse
{
  public required DateTime ExpirationDate { get; set; }

  public required Guid Token { get; set; }

  public required int UserId { get; set; }

  public required string UserName { get; set; }
}
