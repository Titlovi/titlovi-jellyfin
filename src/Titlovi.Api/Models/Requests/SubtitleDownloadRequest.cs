using Refit;
using Titlovi.Api.Models.Enums;

namespace Titlovi.Api.Models.Requests;

public record SubtitleDownloadRequest
{
    [AliasAs("mediaId")]
    public int MediaId { get; init; }

    [AliasAs("type")]
    [Query(Format = "d")]
    public SubtitleType Type { get; init; }
}