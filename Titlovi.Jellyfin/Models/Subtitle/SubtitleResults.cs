using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models.Subtitle;

public class SubtitleResults
{
    [JsonPropertyName("ResultsFound")]
    public int ResultsFound { get; set; }

    [JsonPropertyName("PagesAvailable")]
    public int PagesAvailable { get; set; }

    [JsonPropertyName("CurrentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("SubtitleResults")]
    public Collection<Subtitle> Results { get; init; } = [];
}
