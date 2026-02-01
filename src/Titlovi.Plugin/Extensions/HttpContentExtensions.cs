using System.Threading.Tasks;

namespace Titlovi.Plugin.Extensions;

public static class HttpContentExtensions
{
    public static async Task<MemoryStream> ReadAsMemoryStream(this HttpContent content)
    {
        ArgumentNullException.ThrowIfNull(content);
        return new MemoryStream(await content.ReadAsByteArrayAsync().ConfigureAwait(false));
    }
}
