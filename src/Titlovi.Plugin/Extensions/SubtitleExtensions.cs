using MediaBrowser.Model.Providers;
using Titlovi.Api.Models;

namespace Titlovi.Plugin.Extensions;

public static class SubtitleExtensions
{
    public static RemoteSubtitleInfo ToRemoteSubtitleInfo(this Subtitle subtitle, string providerName)
    {
        ArgumentNullException.ThrowIfNull(subtitle);

        var languageCode = subtitle.Language.FromProviderLanguage();
        return new()
        {
            Id = $"{subtitle.Id}:{languageCode}",
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
