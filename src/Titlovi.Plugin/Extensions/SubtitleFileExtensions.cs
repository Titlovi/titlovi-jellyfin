using MediaBrowser.Controller.Subtitles;
using Titlovi.Api.Models;

namespace Titlovi.Plugin.Extensions;

/// <summary>
/// Extension methods for <seealso cref="SubtitleFile"/> conversion.
/// </summary>
public static class SubtitleFileExtensions
{
    /// <summary>
    /// Converts a subtitle file to a subtitle response with specified language.
    /// </summary>
    /// <param name="file">Subtitle file to convert.</param>
    /// <param name="language">Language code for the subtitle.</param>
    /// <returns>Subtitle response configured with the file's content and metadata.</returns>
    public static SubtitleResponse ToResponse(this SubtitleFile file, string language)
    {
        ArgumentNullException.ThrowIfNull(file);

        return new()
        {
            Format = "srt",
            IsForced = false,
            IsHearingImpaired = false,
            Language = language,
            Stream = file.Buffer,
        };
    }
}
