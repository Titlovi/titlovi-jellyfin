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
            return "English";

        return languageCode.Trim().ToLowerInvariant() switch
        {
            "hr" or "hrv" => "Hrvatski",
            "bs" or "bos" => "Bosanski",
            "en" or "eng" => "English",
            "mk" or "mkd" => "Makedonski",
            "sr" or "srp" => "Srpski",
            "sl" or "slv" => "Slovenski",
            "cyr" or "cir" => "Cirilica",
            _ => "English"
        };
    }
}
