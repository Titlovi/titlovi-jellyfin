using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Titlovi.Jellyfin.Extensions;
using Titlovi.Jellyfin.Interfaces;
using Titlovi.Jellyfin.Models;
using Titlovi.Jellyfin.Models.Subtitle;

namespace Titlovi.Jellyfin;

/// <summary>
/// Used for communicating with the titlovi.com api.
/// </summary>
public class TitloviManager : ITitloviManager
{
    private readonly ILogger<TitloviManager> logger;
    private readonly HttpClient kodiHttpClient;
    private readonly HttpClient titloviHttpClient;

    public TitloviManager(ILogger<TitloviManager> logger, IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;

        this.kodiHttpClient = httpClientFactory.CreateClient("HttpKodiClient");
        this.titloviHttpClient = httpClientFactory.CreateClient("HttpTitloviClient");
    }

    public async Task<TokenInfo?> GetTokenAsync()
    {
        var configuration = TitloviJellyfin.Instance!.Configuration;
        var tokenInfo = configuration.TokenInfo;

        if (tokenInfo is not null && tokenInfo.ExpirationDate >= DateTime.Now)
        {
            return tokenInfo;
        }

        var response = await kodiHttpClient.PostAsync("gettoken?" + configuration.LoginInfo.ToQueryString(), null).ConfigureAwait(false);
        if (response.StatusCode is not HttpStatusCode.OK)
        {
            logger.LogWarning("Failed to authenticate with Titlovi.com");
            return tokenInfo;
        }

        tokenInfo = configuration.TokenInfo = JsonSerializer.Deserialize<TokenInfo>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        TitloviJellyfin.Instance.SaveConfiguration();

        return tokenInfo;
    }

    public async Task<bool> ValidateLoginAsync()
    {
        var body = new StringContent(JsonSerializer.Serialize(TitloviJellyfin.Instance!.Configuration.LoginInfo), Encoding.UTF8, "application/json");
        var response = await kodiHttpClient.PostAsync("validatelogin", body).ConfigureAwait(false);

        if (response.StatusCode is not HttpStatusCode.OK)
        {
            logger.LogWarning("Failed to validate credentials with Titlovi.com");
            return false;
        }

        return true;
    }

    public async Task<SubtitleResults?> SearchAsync(SubtitleSearch subtitleSearch)
    {
        var tokenInfo = await GetTokenAsync().ConfigureAwait(false);
        if (tokenInfo is null)
        {
            return null;
        }

        subtitleSearch.Token = tokenInfo.Token.ToString();
        subtitleSearch.UserId = tokenInfo.UserId;

        var response = await kodiHttpClient.GetAsync("search?" + subtitleSearch.ToQueryString()).ConfigureAwait(false);
        if (response.StatusCode is not HttpStatusCode.OK)
        {
            logger.LogWarning("Failed to validate credentials with Titlovi.com");
            return null;
        }

        return JsonSerializer.Deserialize<SubtitleResults>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
    }

    public async Task<byte[]?> DownloadAsync(SubtitleDownload subtitleDownload)
    {
        var response = await titloviHttpClient.GetAsync($"download?" + subtitleDownload.ToQueryString()).ConfigureAwait(false);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
    }
}
