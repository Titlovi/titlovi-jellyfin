using MediaBrowser.Controller.MediaEncoding;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using Titlovi.Jellyfin.Extensions;
using Titlovi.Jellyfin.Interfaces;

namespace Titlovi.Jellyfin.Providers;

/// <summary>
/// Actual provider that gives jellyfin
/// access to subtitles from titlovi.com.
/// </summary>
public class TitloviSubtitleProvider : ISubtitleProvider
{
    private readonly ILogger<TitloviSubtitleProvider> logger;
    private readonly ITitloviManager titloviManager;
    private readonly IMediaEncoder mediaEncoder;

    public TitloviSubtitleProvider(ILogger<TitloviSubtitleProvider> logger, ITitloviManager titloviManager, IMediaEncoder mediaEncoder)
    {
        this.logger = logger;
        this.titloviManager = titloviManager;
        this.mediaEncoder = mediaEncoder;
    }

    /// <inheritdoc />
    public string Name => "Titlovi.com";

    /// <inheritdoc />
    public IEnumerable<VideoContentType> SupportedMediaTypes => new[]
    {
      VideoContentType.Episode,
      VideoContentType.Movie
    };

    /// <inheritdoc />
    public async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
    {
        return new SubtitleResponse();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("{Name} - {Language}", request.Name, request.Language);


        request.Language.ToProviderLanguage();

        var filePath = request.MediaPath;

        var mediaSource = new MediaSourceInfo
        {
            Id = Guid.NewGuid().ToString(),
            Path = filePath,
            Protocol = MediaProtocol.File,
            Container = System.IO.Path.GetExtension(filePath).TrimStart('.')
        };

        var requestInfo = new MediaInfoRequest
        {
            MediaSource = mediaSource,
            ExtractChapters = false,
            MediaType = DlnaProfileType.Video
        };

        var mediaInfo = await mediaEncoder.GetMediaInfo(requestInfo, cancellationToken);
        foreach (var stream in mediaInfo.MediaStreams)
        {
            switch (stream.Type)
            {
                case MediaStreamType.Video:
                    logger.LogInformation("Video codec: {Codec}-{CodecTag}", stream.Codec, stream.CodecTag);
                    logger.LogInformation("Profile: {Profile}", stream.Profile);
                    logger.LogInformation("Level: {Level}", stream.Level);
                    logger.LogInformation("Bitrate: {BitRate}", stream.BitRate);
                    logger.LogInformation("Resolution: {Width}x{Height}", stream.Width, stream.Height);
                    break;
            }
        }


        return new List<RemoteSubtitleInfo>();
    }
}
