using Titlovi.Jellyfin.Models;
using Titlovi.Jellyfin.Models.Subtitle;

namespace Titlovi.Jellyfin.Interfaces;

/// <summary>
/// Used for communicating with the titlovi.com api.
/// </summary>
public interface ITitloviManager
{
    /// <summary>
    /// Validates the login information.
    /// </summary>
    /// <param name="loginInfo">
    /// Login information that you are trying to validate.
    /// </param>
    Task<bool> ValidateLogin(LoginInfo loginInfo);

    Task<SubtitleResults> SearchAsync(LoginInfo loginInfo, SubtitleSearch subtitleSearch);
}
