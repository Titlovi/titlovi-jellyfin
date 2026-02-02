using System.Text.Json.Serialization;
using Titlovi.Api.Models.Enums;
using Titlovi.Api.Models.Requests;

namespace Titlovi.Api.Models;

/// <summary>
/// Represents a subtitle entry returned from Titlovi.com search results.
/// </summary>
/// <remarks>
/// Contains metadata about a subtitle file including identification, media information,
/// quality metrics, and release details.
/// </remarks>
public sealed class Subtitle
{
    /// <summary>
    /// Gets or sets the unique identifier for the subtitle.
    /// </summary>
    /// <value>Used for downloading the subtitle file.</value>
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the media (movie or TV show).
    /// </summary>
    [JsonPropertyName("Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the release year of the media.
    /// </summary>
    [JsonPropertyName("Year")]
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the media type identifier.
    /// </summary>
    /// <value>1 = Movie, 2 = TV Episode.</value>
    [JsonPropertyName("Type")]
    public SubtitleType Type { get; set; }

    /// <summary>
    /// Gets or sets the web link to the subtitle page on Titlovi.com.
    /// </summary>
    [JsonPropertyName("Link")]
    public string Link { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the season number for TV episodes.
    /// </summary>
    /// <value>0 for movies, season number for TV episodes.</value>
    [JsonPropertyName("Season")]
    public int Season { get; set; }

    /// <summary>
    /// Gets or sets the episode number for TV episodes.
    /// </summary>
    /// <value>0 for movies, episode number for TV episodes.</value>
    [JsonPropertyName("Episode")]
    public int Episode { get; set; }

    /// <summary>
    /// Gets or sets the special episode indicator.
    /// </summary>
    /// <value>Used to identify special episodes or movies.</value>
    [JsonPropertyName("Special")]
    public int Special { get; set; }

    /// <summary>
    /// Gets or sets the language of the subtitle in provider format.
    /// </summary>
    /// <value>Language name as used by Titlovi.com (e.g., "Hrvatski", "English").</value>
    [JsonPropertyName("Lang")]
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the subtitle was uploaded.
    /// </summary>
    [JsonPropertyName("Date")]
    public DateTime Date { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the number of times the subtitle has been downloaded.
    /// </summary>
    /// <value>Higher values typically indicate better quality or popularity.</value>
    [JsonPropertyName("DownloadCount")]
    public int DownloadCount { get; set; }

    /// <summary>
    /// Gets or sets the community rating for the subtitle quality.
    /// </summary>
    /// <value>Rating score indicating subtitle quality as rated by users.</value>
    [JsonPropertyName("Rating")]
    public float Rating { get; set; }

    /// <summary>
    /// Gets or sets release information about the media source.
    /// </summary>
    /// <value>Contains details like quality, source, and release group (e.g., "BluRay.1080p.x264").</value>
    [JsonPropertyName("Release")]
    public string Release { get; set; } = string.Empty;

    public SubtitleMetadata ToMetadata() => new(Id, Type, Language, Season, Episode);
}