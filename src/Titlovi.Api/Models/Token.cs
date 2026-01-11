using System.Text.Json.Serialization;

namespace Titlovi.Api.Models;

public sealed class Token
{
    [JsonPropertyName("Token")]
    public required string Id { get; set; }

    public required int UserId { get; set; }

    public required string UserName { get; set; }

    public required DateTime ExpirationDate { get; set; }
}