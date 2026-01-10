using Refit;
using Titlovi.Api.Models;

namespace Titlovi.Api;

public interface IKodiClient
{
  [Post("gettoken")]
  Task<TokenResponse> GetToken(string username, string password);

  [Post("validatelogin")]
  Task<IApiResponse> ValidateLogin([Body] LoginRequest request);
}
