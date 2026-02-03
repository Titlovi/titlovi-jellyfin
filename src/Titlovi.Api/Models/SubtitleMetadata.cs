using Titlovi.Api.Models.Enums;
using Titlovi.Api.Models.Requests;

namespace Titlovi.Api.Models;

public record SubtitleMetadata(int Id, SubtitleType Type, string Language, int Season, int Episode)
{
    public SubtitleDownloadRequest ToDownloadRequest() => new(Id, Type);
}