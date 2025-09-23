using System.Text.Json.Serialization;

namespace Titlovi.Jellyfin.Models.Subtitle;

/// <summary>
/// Contains search parameters for finding subtitles on Titlovi.com.
/// </summary>
/// <remarks>
/// Supports searching by text query, IMDB ID, or specific episode information.
/// All properties are nullable to allow flexible search criteria.
/// </remarks>
public class SubtitleSearch
{
    /// <summary>
    /// Gets or sets the text search query.
    /// </summary>
    /// <value>Used for TV show names or movie titles. Null for IMDB-based searches.</value>
    [JsonPropertyName("query")]
    public string? Query { get; set; }

    /// <summary>
    /// Gets or sets the media type to search for.
    /// </summary>
    /// <value>1 = Movie, 2 = TV Episode</value>
    [JsonPropertyName("type")]
    public int? Type { get; set; }

    /// <summary>
    /// Gets or sets the season number for TV episode searches.
    /// </summary>
    /// <value>Required when searching for TV episodes (Type = 2).</value>
    [JsonPropertyName("season")]
    public int? Season { get; set; }

    /// <summary>
    /// Gets or sets the episode number for TV episode searches.
    /// </summary>
    /// <value>Required when searching for TV episodes (Type = 2).</value>
    [JsonPropertyName("episode")]
    public int? Episode { get; set; }

    /// <summary>
    /// Gets or sets the IMDB identifier for the media.
    /// </summary>
    /// <value>Used for movie searches instead of text query. Format: "tt1234567"</value>
    [JsonPropertyName("imdbid")]
    public string? ImdbId { get; set; }

    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary>
    /// <value>1-based page index for retrieving additional result pages.</value>
    [JsonPropertyName("pg")]
    public int? Page { get; set; }

    /// <summary>
    /// Gets or sets whether to ignore language and episode filtering.
    /// </summary>
    /// <value>When true, returns broader search results ignoring some constraints.</value>
    [JsonPropertyName("ignoreLangAndEpisode")]
    public bool? IgnoreLangAndEpisode { get; set; }

    /// <summary>
    /// Gets or sets the preferred subtitle language.
    /// </summary>
    /// <value>Language in provider format (e.g., "Hrvatski", "English").</value>
    [JsonPropertyName("lang")]
    public string? Lang { get; set; }

    /// <summary>
    /// Gets or sets the authentication token for the API request.
    /// </summary>
    /// <value>Required for authenticated searches. Obtained from <see cref="TokenInfo.Token"/>.</value>
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the user identifier for the API request.
    /// </summary>
    /// <value>Required for authenticated searches. Obtained from <see cref="TokenInfo.UserId"/>.</value>
    [JsonPropertyName("userid")]
    public int? UserId { get; set; }
}
