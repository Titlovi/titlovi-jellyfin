using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;

namespace Titlovi.Jellyfin.Providers;

/// <summary>
/// Actual provider that gives jellyfin
/// access to subtitles from titlovi.com.
/// </summary>
public class TitloviSubtitlesProvider : ISubtitleProvider
{
    /// <inheritdoc />
    public string Name => throw new NotImplementedException();

    /// <inheritdoc />
    public IEnumerable<VideoContentType> SupportedMediaTypes => throw new NotImplementedException();

    /// <inheritdoc />
    public Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
