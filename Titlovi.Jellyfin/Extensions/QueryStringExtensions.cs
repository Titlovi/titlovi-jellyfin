namespace Titlovi.Jellyfin.Extensions;

/// <summary>
/// Extension methods for converting objects to query string format
/// </summary>
public static class QueryStringExtensions
{
    /// <summary>
    /// Converts an object's properties to a URL-encoded query string
    /// </summary>
    /// <typeparam name="T">The type of object to convert</typeparam>
    /// <param name="obj">The object instance to convert to query string</param>
    /// <returns>A URL-encoded query string (without the leading '?')</returns>
    /// <example>
    /// var person = new { Name = "John Doe", Age = 30, City = "New York" };
    /// string queryString = person.ToQueryString();
    /// // Result: "Name=John%20Doe&Age=30&City=New%20York"
    /// </example>
    public static string ToQueryString<T>(this T obj)
    {
        return string.Join("&", from p in typeof(T).GetProperties()
                                let value = p.GetValue(obj, null)
                                where value != null
                                select $"{Uri.EscapeDataString(p.Name)}={value.ToString()!}");
    }
}
