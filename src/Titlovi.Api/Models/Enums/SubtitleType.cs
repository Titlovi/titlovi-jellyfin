namespace Titlovi.Api.Models.Enums;

/// <summary>
/// Specifies the type of media content for subtitle searches.
/// </summary>
public enum SubtitleType
{
    /// <summary>
    /// None.
    /// </summary>
    None = 0,

    /// <summary>
    /// Subtitle is for a movie.
    /// </summary>
    Movie = 1,

    /// <summary>
    /// Subtitle is for a TV show episode.
    /// </summary>
    Episode = 2
}