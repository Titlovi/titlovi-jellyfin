using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Titlovi.Api.Extensions;

public class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
#if DEBUG
        Console.WriteLine(request.RequestUri);
#endif
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