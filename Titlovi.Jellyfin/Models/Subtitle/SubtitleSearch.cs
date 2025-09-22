using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models.Subtitle;

public class SubtitleSearch
{
    [JsonPropertyName("query")]
    public string? Query { get; set; }

    [JsonPropertyName("season")]
    public int? Season { get; set; }

    [JsonPropertyName("episode")]
    public int? Episode { get; set; }

    [JsonPropertyName("imdbid")]
    public string? ImdbId { get; set; }

    [JsonPropertyName("pg")]
    public int? Page { get; set; }
}
