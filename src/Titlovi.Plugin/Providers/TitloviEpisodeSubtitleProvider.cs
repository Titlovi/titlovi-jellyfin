using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;

namespace Titlovi.Plugin.Providers;

public sealed class TitloviEpisodeSubtitleProvider() : TitloviSubtitleProvider("Titlovi.com - Episodes", VideoContentType.Episode)
{
    public override Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
