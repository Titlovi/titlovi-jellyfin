using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Titlovi.Api.Extensions;

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
    services.AddRefitClient<IKodiClient>().ConfigureHttpClient(client => ConfigureClient(client, "https://kodi.titlovi.com/api/subtitles/"));
    services.AddRefitClient<ITitloviClient>().ConfigureHttpClient(client => ConfigureClient(client, "https://titlovi.com/"));
    return services;
  }
}
