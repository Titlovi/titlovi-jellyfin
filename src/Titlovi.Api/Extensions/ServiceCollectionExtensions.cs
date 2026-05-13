using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;
using System.Text.RegularExpressions;

namespace Titlovi.Api.Extensions;

public partial class LoggingHandler(ILogger<LoggingHandler> logger) : DelegatingHandler
{

    [GeneratedRegex(@"[&?]token=[0-9a-fA-F\-]+")]
    private static partial Regex TokenExtractRegex();

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var rawRequest = TokenExtractRegex().Replace(request.RequestUri!.ToString(), "");
        logger.LogInformation("SearchRequest: {RawRequest}", rawRequest);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

}

public static class ServiceCollectionExtensions
{
    private static void ConfigureClient(HttpClient client, string url)
    {
        client.BaseAddress = new Uri(url);

        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("X-App", "titlovi-jellyfin");
    }

    public static IServiceCollection AddTitloviApi(this IServiceCollection services)
    {

        services.AddTransient<LoggingHandler>();

        services.AddRefitClient<IKodiClient>().ConfigureHttpClient(client => ConfigureClient(client, "https://kodi.titlovi.com/api/subtitles/")).AddHttpMessageHandler<LoggingHandler>();
        services.AddRefitClient<ITitloviClient>().ConfigureHttpClient(client => ConfigureClient(client, "https://titlovi.com/")).AddHttpMessageHandler<LoggingHandler>();
        return services;
    }
}