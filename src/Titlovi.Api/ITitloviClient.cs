using Refit;
using Titlovi.Api.Models.Requests;

namespace Titlovi.Api;

public interface ITitloviClient
{
    [Get("/download")]
    public Task<ApiResponse<HttpContent>> DownloadSubtitle([Query] SubtitleDownloadRequest request);
}