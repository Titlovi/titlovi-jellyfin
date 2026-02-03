namespace Titlovi.Plugin.Extensions;

/// <summary>
/// Extension methods for string operations related to language conversion.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts ISO language code to Titlovi provider language name.
    /// </summary>
    /// <param name="languageCode">ISO 639-1 or 639-2 language code (e.g., "hr", "hrv").</param>
    /// <returns>Provider-specific language name, or empty string if not supported.</returns>
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

    /// <summary>
    /// Converts Titlovi provider language name to ISO 639-2 language code.
    /// </summary>
    /// <param name="languageName">Provider-specific language name (e.g., "Hrvatski").</param>
    /// <returns>ISO 639-2 language code, or empty string if not recognized.</returns>
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
            "cirilica" => "srp", // Cyrillic script mapped to Serbian
            _ => string.Empty
        };
    }
}