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

    [JsonPropertyName("ignoreLangAndEpisode")]
    public bool? IgnoreLangAndEpisode { get; set; }

    [JsonPropertyName("lang")]
    public string? Lang { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("userid")]
    public int? UserId { get; set; }
}
