using Refit;
using Titlovi.Api.Models.Enums;

namespace Titlovi.Api.Models.Requests;

public record SubtitleDownloadRequest(
    [AliasAs("mediaId")] int MediaId,
    [AliasAs("type")] SubtitleType Type
);