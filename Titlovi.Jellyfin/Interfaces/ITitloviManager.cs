using System.Collections.ObjectModel;
using Titlovi.Jellyfin.Models;
using Titlovi.Jellyfin.Models.Subtitle;

namespace Titlovi.Jellyfin.Interfaces;

/// <summary>
/// Interface for managing authentication and subtitle operations with the Titlovi.com API.
/// </summary>
/// <remarks>
/// Provides core functionality for interacting with the Titlovi.com subtitle service,
/// including user authentication, subtitle searching, downloading, and archive extraction.
/// </remarks>
public interface ITitloviManager
{
    /// <summary>
    /// Authenticates with the Titlovi.com API and retrieves an access token.
    /// </summary>
    /// <returns>
    /// A <see cref="TokenInfo"/> containing the authentication token and user information,
    /// or null if authentication fails.
    /// </returns>
    /// <remarks>
    /// Uses the configured login credentials to obtain a session token required for API operations.
    /// Token should be cached and reused until it expires.
    /// </remarks>
    Task<TokenInfo?> GetTokenAsync();

    /// <summary>
    /// Validates the current login credentials against the Titlovi.com API.
    /// </summary>
    /// <returns>
    /// True if the login credentials are valid and can authenticate successfully;
    /// otherwise, false.
    /// </returns>
    /// <remarks>
    /// Used to verify user credentials before attempting other API operations.
    /// Does not retrieve or update authentication tokens.
    /// </remarks>
    Task<bool> ValidateLoginAsync();

    /// <summary>
    /// Searches for subtitles matching the specified criteria.
    /// </summary>
    /// <param name="subtitleSearch">Search parameters including media identifiers, language, and content type.</param>
    /// <returns>
    /// A <see cref="SubtitleResults"/> containing matching subtitles and pagination information,
    /// or null if the search fails or no results are found.
    /// </returns>
    /// <remarks>
    /// Requires a valid authentication token. Results may be paginated for large result sets.
    /// </remarks>
    Task<SubtitleResults?> SearchAsync(SubtitleSearch subtitleSearch);

    /// <summary>
    /// Downloads subtitle file data from Titlovi.com.
    /// </summary>
    /// <param name="subtitleDownload">Download parameters specifying the media ID and subtitle type.</param>
    /// <returns>
    /// Raw byte array containing the downloaded subtitle archive,
    /// or the subtitle file. Null if the download fails.
    /// </returns>
    /// <remarks>
    /// Downloads are typically compressed archives (ZIP/RAR) that require extraction
    /// using <see cref="ExtractSubtitles"/> to access the subtitle files.
    /// </remarks>
    Task<byte[]?> DownloadAsync(SubtitleDownload subtitleDownload);

    /// <summary>
    /// Extracts subtitle files from a downloaded compressed archive.
    /// </summary>
    /// <param name="buffer">Raw byte data of the compressed archive downloaded from Titlovi.com.</param>
    /// <returns>
    /// List of byte arrays, each representing an individual subtitle file.
    /// Returns empty list if extraction fails or no subtitle files are found.
    /// </returns>
    /// <remarks>
    /// Handles decompression of various archive formats (ZIP, RAR, etc.) and extracts
    /// subtitle files (typically .srt format). Multiple subtitle files may be present
    /// in a single archive for different languages or versions.
    /// </remarks>
    Collection<(string Path, byte[] Buffer)> ExtractSubtitles(byte[] buffer);
}
