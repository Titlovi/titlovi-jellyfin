namespace Titlovi.Jellyfin.Extensions;

/// <summary>
/// Extension methods for converting language code (ISO 639-1 or 639-2)
/// to the target language names used by the titlovi.com subtitle provider.
/// </summary>
public static class LanguageExtension
{
    /// <summary>
    /// Converts a standard language code (ISO 639-1 or 639-2) to the target language names used by the titlovi.com subtitle provider.
    /// </summary>
    public static string ToProviderLanguage(this string? languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            return string.Empty;
        }

        return languageCode.Trim().ToLowerInvariant() switch
        {
            "hr" or "hrv" => "Hrvatski",
            "bs" or "bos" => "Bosanski",
            "en" or "eng" => "English",
            "mk" or "mkd" => "Makedonski",
            "sr" or "srp" => "Srpski",
            "sl" or "slv" => "Slovenski",
            "cyr" or "cir" => "Cirilica",
            _ => string.Empty
        };
    }

    public static string FromProviderLanguage(this string? languageName)
    {
        if (string.IsNullOrWhiteSpace(languageName))
        {
            return string.Empty;
        }

        return languageName.Trim().ToLowerInvariant() switch
        {
            "hrvatski" => "hrv",
            "bosanski" => "bos",
            "english" => "eng",
            "makedonski" => "mkd",
            "srpski" => "srp",
            "slovenski" => "slv",
            "cirilica" => "srp",
            _ => string.Empty
        };
    }
}
