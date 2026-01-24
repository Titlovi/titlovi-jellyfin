using Refit;
using Titlovi.Api.Models.Enums;

namespace Titlovi.Api.Models.Requests;

/// <summary>
/// Contains parameters required for downloading a subtitle file from Titlovi.com.
/// </summary>
public sealed class SubtitleDownloadRequest
{
    /// <summary>
    /// Gets or sets the media identifier for the subtitle.
    /// </summary>
    /// <value>Corresponds to the <see cref="Subtitle.Id"/> from search results.</value>
    [AliasAs("mediaid")]
    public int MediaId { get; set; }

    /// <summary>
    /// Gets or sets the media type for the subtitle.
    /// </summary>
    /// <value>1 = Movie, 2 = TV Episode. Must match the <see cref="Subtitle.Type"/>.</value>
    [AliasAs("type")]
    public SubtitleType Type { get; set; }
}