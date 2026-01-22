using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;

namespace Titlovi.Plugin.Providers;

public sealed class TitloviSubtitleProvider : ISubtitleProvider
{
    public string Name => "Titlovi.com";

    public IEnumerable<VideoContentType> SupportedMediaTypes =>
    [
        VideoContentType.Episode,
        VideoContentType.Movie
    ];

    public Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}