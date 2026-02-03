using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Titlovi.Api.Models;

/// <summary>
/// Represents the response from a subtitle search operation.
/// </summary>
/// <remarks>
/// Contains search results with pagination information and the collection of matching subtitles.
/// </remarks>
public class SubtitlePage
{
    /// <summary>
    /// Gets or sets the total number of subtitle results found.
    /// </summary>
    /// <value>Total count across all pages, not just the current page.</value>
    [JsonPropertyName("ResultsFound")]
    public int ResultsFound { get; set; }

    /// <summary>
    /// Gets or sets the total number of result pages available.
    /// </summary>
    /// <value>Used for pagination when there are many results.</value>
    [JsonPropertyName("PagesAvailable")]
    public int PagesAvailable { get; set; }

    /// <summary>
    /// Gets or sets the current page number being returned.
    /// </summary>
    /// <value>1-based page index for the current result set.</value>
    [JsonPropertyName("CurrentPage")]
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets the collection of subtitle results for the current page.
    /// </summary>
    /// <value>List of <see cref="Subtitle"/> objects matching the search criteria.</value>
    [JsonPropertyName("SubtitleResults")]
    public ICollection<Subtitle> Results { get; init; } = [];
}
