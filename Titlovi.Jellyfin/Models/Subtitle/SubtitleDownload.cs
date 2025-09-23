using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models.Subtitle;

public class SubtitleDownload
{
    [JsonPropertyName("mediaid")]
    public int MediaId { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; }
}
