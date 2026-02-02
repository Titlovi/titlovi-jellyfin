using MediaBrowser.Controller.Subtitles;
using Titlovi.Api.Models;

namespace Titlovi.Plugin.Extensions;

public static class SubtitleFileExtensions
{
    public static SubtitleResponse ToResponse(this SubtitleFile file, string language)
    {
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
