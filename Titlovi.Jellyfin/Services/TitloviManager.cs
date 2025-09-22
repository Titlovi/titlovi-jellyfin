using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging;
using Titlovi.Jellyfin.Interfaces;
using Titlovi.Jellyfin.Models;
using Titlovi.Jellyfin.Models.Subtitle;

namespace Titlovi.Jellyfin.Services;

/// <summary>
/// Used for communicating with the titlovi.com api.
/// </summary>
public class TitloviManager : ITitloviManager, IDisposable
{
    private readonly ILogger<TitloviManager> logger;

    private bool disposed;
    private HttpClient httpClient;

    public TitloviManager(ILogger<TitloviManager> logger)
    {
        this.logger = logger;

        this.disposed = false;
        this.httpClient = new HttpClient();

        this.httpClient.BaseAddress = new Uri("https://kodi.titlovi.com/api/subtitles");
        this.httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");
    }

    /// <summary>
    /// Validates the login information.
    /// </summary>
    /// <param name="loginInfo">
    /// Login information that you are trying to validate.
    /// </param>
    public async Task<bool> ValidateLogin(LoginInfo loginInfo)
    {
        var content = new StringContent(JsonSerializer.Serialize(loginInfo), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/validatelogin", content).ConfigureAwait(false);

        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<SubtitleResults> SearchAsync(LoginInfo loginInfo, SubtitleSearch subtitleSearch)
    {
        var uriBuilder = new UriBuilder("https://kodi.titlovi.com/api/subtitles/search");
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        if (!string.IsNullOrEmpty(subtitleSearch.Query))
        {
            query["query"] = subtitleSearch.Query;
        }
        else if (!string.IsNullOrEmpty(subtitleSearch.ImdbId))
        {
            query["imdbid"] = subtitleSearch.ImdbId;
        }

        if (subtitleSearch.Season is not null)
        {
            query["season"] = Convert.ToString(subtitleSearch.Season, CultureInfo.InvariantCulture);
        }
        if (subtitleSearch.Episode is not null)
        {
            query["episode"] = Convert.ToString(subtitleSearch.Episode, CultureInfo.InvariantCulture);
        }
        if (subtitleSearch.Page is not null)
        {
            query["pg"] = Convert.ToString(subtitleSearch.Page, CultureInfo.InvariantCulture);
        }

        uriBuilder.Query = query.ToString();

        logger.LogInformation("{Uri}", uriBuilder.Uri);

        using (var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri))
        {
            request.Headers.Add("username", loginInfo.Username);
            request.Headers.Add("password", loginInfo.Password);

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new SubtitleResults { };
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (content is null)
            {
                return new SubtitleResults { };
            }

            return JsonSerializer.Deserialize<SubtitleResults>(content)!;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the TitloviManager and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    /// <remarks>
    /// This method implements the standard dispose pattern for classes that hold disposable resources.
    /// The disposed flag prevents multiple disposal calls and ensures the HttpClient is properly cleaned up.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                httpClient.Dispose();
            }

            disposed = true;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>
    /// This method implements the IDisposable interface and follows the standard dispose pattern.
    /// It calls the protected Dispose method and suppresses finalization since this class
    /// doesn't hold unmanaged resources directly (HttpClient handles its own finalization).
    ///
    /// Always call this method when finished with the TitloviManager instance to ensure
    /// proper cleanup of the underlying HttpClient and its connections.
    /// </remarks>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
