namespace Titlovi.Jellyfin.Extensions;

/// <summary>
/// Extension methods for converting objects to query string format.
/// </summary>
public static class QueryStringExtensions
{
    /// <summary>
    /// Converts an object's properties to a URL-encoded query string.
    /// </summary>
    /// <typeparam name="T">The type of object to convert.</typeparam>
    /// <param name="obj">The object to convert.</param>
    /// <returns>URL-encoded query string without leading '?'.</returns>
    public static string ToQueryString<T>(this T obj)
    {
        return string.Join("&", from p in typeof(T).GetProperties()
                                let value = p.GetValue(obj, null)
                                where value != null
                                select $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(value.ToString()!)}");
    }
}
