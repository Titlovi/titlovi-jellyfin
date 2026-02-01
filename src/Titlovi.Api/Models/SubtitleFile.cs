namespace Titlovi.Api.Models;

public record SubtitleFile(
    string Path,
    MemoryStream Buffer
);
