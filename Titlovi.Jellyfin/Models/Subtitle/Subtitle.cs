using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models.Subtitle;

public class Subtitle
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("Year")]
    public int Year { get; set; }

    [JsonPropertyName("Type")]
    public int Type { get; set; }

    [JsonPropertyName("Link")]
    public string Link { get; set; } = string.Empty;

    [JsonPropertyName("Season")]
    public int Season { get; set; }

    [JsonPropertyName("Episode")]
    public int Episode { get; set; }

    [JsonPropertyName("Special")]
    public int Special { get; set; }

    [JsonPropertyName("Lang")]
    public string Language { get; set; } = string.Empty;

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; } = DateTime.Now;

    [JsonPropertyName("DownloadCount")]
    public int DownloadCount { get; set; }

    [JsonPropertyName("Rating")]
    public float Rating { get; set; }

    [JsonPropertyName("Release")]
    public string Release { get; set; } = string.Empty;
}
