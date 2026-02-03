using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Globalization;
using System.Reflection;
using Titlovi.Api.Models.Enums;

namespace Titlovi.Api.Extensions;

public class IntEnumParameterFormatter : IUrlParameterFormatter
{
    public string? Format(object? value, ICustomAttributeProvider attributeProvider, Type type)
    {
        if (value == null || type == null)
            return null;

        var valueType = value.GetType();
        if (!valueType.IsEnum)
            return value.ToString();

        var underlyingType = Enum.GetUnderlyingType(valueType);
        var numericValue = Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);

        return numericValue.ToString();
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
        var settings = new RefitSettings { UrlParameterFormatter = new IntEnumParameterFormatter() };
        services.AddRefitClient<IKodiClient>(settings).ConfigureHttpClient(client => ConfigureClient(client, "https://kodi.titlovi.com/api/subtitles/"));
        services.AddRefitClient<ITitloviClient>(settings).ConfigureHttpClient(client => ConfigureClient(client, "https://titlovi.com/"));
        return services;
    }
}