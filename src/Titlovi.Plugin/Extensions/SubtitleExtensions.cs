using MediaBrowser.Model.Providers;
using System.Text;
using System.Text.Json;
using Titlovi.Api.Models;

namespace Titlovi.Plugin.Extensions;

/// <summary>
/// Extension methods for <seealso cref="Subtitle"/> conversion.
/// </summary>
public static class SubtitleExtensions
{
    /// <summary>
    /// Converts a subtitle to remote subtitle info with serialized metadata.
    /// </summary>
    /// <param name="subtitle">Subtitle to convert.</param>
    /// <param name="providerName">Name of the subtitle provider.</param>
    /// <returns>Remote subtitle info with encoded metadata and properties.</returns>
    public static RemoteSubtitleInfo ToRemoteSubtitleInfo(this Subtitle subtitle, string providerName)
    {
        ArgumentNullException.ThrowIfNull(subtitle);

        var languageCode = subtitle.Language.FromProviderLanguage();
        return new()
        {
            Id = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(subtitle.ToMetadata()))),
            AiTranslated = false,
            DateCreated = subtitle.Date,
            DownloadCount = subtitle.DownloadCount,
            Name = subtitle.Title,
            ProviderName = providerName,
            CommunityRating = subtitle.Rating,
            Comment = subtitle.Release,
            Author = string.Empty,
            ThreeLetterISOLanguageName = languageCode
        };
    }
}