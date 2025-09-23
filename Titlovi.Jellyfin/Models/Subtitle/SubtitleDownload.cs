using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models.Subtitle;

/// <summary>
/// Contains parameters required for downloading a subtitle file from Titlovi.com.
/// </summary>
public class SubtitleDownload
{
    /// <summary>
    /// Gets or sets the media identifier for the subtitle.
    /// </summary>
    /// <value>Corresponds to the <see cref="Subtitle.Id"/> from search results.</value>
    [JsonPropertyName("mediaid")]
    public int MediaId { get; set; }

    /// <summary>
    /// Gets or sets the media type for the subtitle.
    /// </summary>
    /// <value>1 = Movie, 2 = TV Episode. Must match the <see cref="Subtitle.Type"/>.</value>
    [JsonPropertyName("type")]
    public int Type { get; set; }
}
