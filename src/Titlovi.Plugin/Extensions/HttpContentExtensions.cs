namespace Titlovi.Plugin.Extensions;

/// <summary>
/// Extension methods for HttpContent operations.
/// </summary>
public static class HttpContentExtensions
{
    /// <summary>
    /// Reads HTTP content as a memory stream.
    /// </summary>
    /// <param name="content"><seealso cref="HttpContent"/>.</param>
    /// <returns><seealso cref="MemoryStream"/> containing all the content.</returns>
    public static async Task<MemoryStream> ReadAsMemoryStream(this HttpContent content)
    {
        ArgumentNullException.ThrowIfNull(content);
        return new MemoryStream(await content.ReadAsByteArrayAsync().ConfigureAwait(false));
    }
}
