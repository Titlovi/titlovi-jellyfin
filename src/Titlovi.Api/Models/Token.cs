namespace Titlovi.Api.Models;

public sealed class Token
{
  public required Guid Id { get; set; }

  public required int UserId { get; set; }

  public required string UserName { get; set; }

  public required DateTime ExpirationDate { get; set; }
}
