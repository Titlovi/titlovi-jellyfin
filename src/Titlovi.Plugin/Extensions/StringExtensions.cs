namespace Titlovi.Plugin.Extensions;

/// <summary>
/// Extension methods for converting between standard language codes (ISO 639-1/639-2)
/// and localized language names used by the titlovi.com subtitle provider.
/// </summary>
/// <remarks>
/// This class provides bidirectional conversion between:
/// - Standard ISO 639-1 (2-letter) and ISO 639-2 (3-letter) language codes
/// - Localized language names in their native languages as used by titlovi.com
///
/// Supported languages include South Slavic languages (Croatian, Bosnian, Serbian,
/// Slovenian, Macedonian) and English, with special handling for Cyrillic script.
/// </remarks>
public static class StringExtensions
{
    /// <summary>
    /// Converts a standard language code (ISO 639-1 or 639-2) to the localized language
    /// name used by the titlovi.com subtitle provider.
    /// </summary>
    /// <param name="languageCode">
    /// The ISO language code to convert. Supports both 2-letter (ISO 639-1) and
    /// 3-letter (ISO 639-2) codes. Case-insensitive with automatic trimming.
    /// </param>
    /// <returns>
    /// The localized language name in the target language, or an empty string if the
    /// language code is not supported or is null/empty/whitespace.
    /// </returns>
    /// <remarks>
    /// Supported language codes and their corresponding names:
    /// - "hr"/"hrv" → "Hrvatski" (Croatian)
    /// - "bs"/"bos" → "Bosanski" (Bosnian)
    /// - "en"/"eng" → "English" (English)
    /// - "mk"/"mkd" → "Makedonski" (Macedonian)
    /// - "sr"/"srp" → "Srpski" (Serbian)
    /// - "sl"/"slv" → "Slovenski" (Slovenian)
    /// - "cyr"/"cir" → "Cirilica" (Cyrillic script).
    /// </remarks>
    /// <example>
    /// <code>
    /// string croatianName = "hr".ToProviderLanguage(); // Returns "Hrvatski"
    /// string englishName = "ENG".ToProviderLanguage(); // Returns "English"
    /// string unknown = "fr".ToProviderLanguage(); // Returns ""
    /// </code>
    /// </example>
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
    /// Converts a localized language name from titlovi.com back to its corresponding
    /// ISO 639-2 (3-letter) language code.
    /// </summary>
    /// <param name="languageName">
    /// The localized language name to convert. Case-insensitive with automatic trimming.
    /// Expected to be in the native language (e.g., "Hrvatski", "English").
    /// </param>
    /// <returns>
    /// The corresponding ISO 639-2 (3-letter) language code, or an empty string if the
    /// language name is not recognized or is null/empty/whitespace.
    /// </returns>
    /// <remarks>
    /// Supported language names and their corresponding codes:
    /// - "Hrvatski" → "hrv" (Croatian)
    /// - "Bosanski" → "bos" (Bosnian)
    /// - "English" → "eng" (English)
    /// - "Makedonski" → "mkd" (Macedonian)
    /// - "Srpski" → "srp" (Serbian)
    /// - "Slovenski" → "slv" (Slovenian)
    /// - "Cirilica" → "srp" (Cyrillic script, mapped to Serbian)
    ///
    /// Note: "Cirilica" (Cyrillic) is mapped to "srp" (Serbian) as it represents
    /// Serbian content in Cyrillic script rather than a separate language.
    /// </remarks>
    /// <example>
    /// <code>
    /// string code = "Hrvatski".FromProviderLanguage(); // Returns "hrv"
    /// string englishCode = "english".FromProviderLanguage(); // Returns "eng"
    /// string unknown = "Français".FromProviderLanguage(); // Returns ""
    /// </code>
    /// </example>
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