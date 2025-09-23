using Titlovi.Jellyfin.Models;
using Titlovi.Jellyfin.Models.Subtitle;

namespace Titlovi.Jellyfin.Interfaces;

/// <summary>
/// Used for communicating with the titlovi.com api.
/// </summary>
public interface ITitloviManager
{
    /// <summary>
    /// Gets the token information based on
    /// the provided login detials from the config.
    /// </summary>
    Task<TokenInfo?> GetTokenAsync();

    /// <summary>
    /// Validates the login information.
    /// </summary>
    Task<bool> ValidateLoginAsync();

    /// <summary>
    /// Queires the titlovi database with
    /// the provided search parameters.
    /// </summary>
    Task<SubtitleResults?> SearchAsync(SubtitleSearch subtitleSearch);

    /// <summary>
    /// Downlodas the subtitle file from the
    /// titlovi api based on the provided download information
    /// </summary>
    Task<byte[]?> DownloadAsync(SubtitleDownload subtitleDownload);
}
