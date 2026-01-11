using Refit;
using Titlovi.Api.Models;
using Titlovi.Api.Models.Requests;

namespace Titlovi.Api;

public interface IKodiClient
{
    [Post("/gettoken")]
    Task<Token> GetToken([Query] string username, [Query] string password);

    [Post("/validatelogin")]
    Task<IApiResponse> ValidateLogin([Body] LoginRequest request);
}