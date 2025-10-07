using System.Collections.ObjectModel;
using System.IO.Compression;
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
/// Implementation of <see cref="ITitloviManager"/> that handles communication with the Titlovi.com API.
/// </summary>
/// <remarks>
/// Uses separate HTTP clients for different API endpoints and provides automatic token management
/// with caching and refresh functionality.
/// </remarks>
public class TitloviManager : ITitloviManager
{
    private readonly ILogger<TitloviManager> logger;
    private readonly HttpClient kodiHttpClient;
    private readonly HttpClient titloviHttpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="TitloviManager"/> class.
    /// </summary>
    /// <param name="logger">Logger for recording API operations and errors.</param>
    /// <param name="httpClientFactory">Factory for creating configured HTTP clients.</param>
    public TitloviManager(ILogger<TitloviManager> logger, IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;

        this.kodiHttpClient = httpClientFactory.CreateClient("HttpKodiClient");
        this.titloviHttpClient = httpClientFactory.CreateClient("HttpTitloviClient");
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<byte[]?> DownloadAsync(SubtitleDownload subtitleDownload)
    {
        var response = await titloviHttpClient.GetAsync("download?" + subtitleDownload.ToQueryString()).ConfigureAwait(false);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Collection<(string Path, byte[] Buffer)> ExtractSubtitles(byte[] buffer)
    {
        try
        {
            using (var memoryStream = new MemoryStream(buffer))
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
            {
                return new Collection<(string, byte[])>(archive.Entries
                    .Where(entry => entry.Name.EndsWith(".srt", StringComparison.OrdinalIgnoreCase))
                    .Select(entry =>
                    {
                        using (var entryStream = entry.Open())
                        using (var extractedStream = new MemoryStream())
                        {
                            entryStream.CopyTo(extractedStream);
                            return (entry.Name, extractedStream.ToArray());
                        }
                    })
                    .ToList());
            }
        }
        catch (Exception)
        {
            return new Collection<(string, byte[])> { (string.Empty, buffer) };
        }
    }
}
