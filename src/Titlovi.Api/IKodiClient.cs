using Refit;
using Titlovi.Api.Models;
using Titlovi.Api.Models.Requests;

namespace Titlovi.Api;

/// <summary>
/// Client interface for communicating with the Kodi/Titlovi subtitle service API.
/// </summary>
public interface IKodiClient
{
    /// <summary>
    /// Requests an authentication token using username and password credentials.
    /// </summary>
    /// <param name="username">User's username.</param>
    /// <param name="password">User's password.</param>
    /// <returns>Authentication token for subsequent API requests.</returns>
    [Post("/gettoken")]
    Task<Token> GetToken([Query] string username, [Query] string password);

    /// <summary>
    /// Validates user login credentials against the service.
    /// </summary>
    /// <param name="request">Login credentials.</param>
    /// <returns>API response indicating validation result.</returns>
    [Post("/validatelogin")]
    Task<IApiResponse> ValidateLogin([Body] LoginRequest request);

    [Get("/search")]
    Task<SubtitlePage> Search([Query] SearchSubtitleRequest request);
}