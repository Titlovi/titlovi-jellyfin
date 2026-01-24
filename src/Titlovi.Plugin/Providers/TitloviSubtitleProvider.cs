using MediaBrowser.Controller.MediaEncoding;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Providers;
using System.Collections.ObjectModel;
using System.IO.Compression;

namespace Titlovi.Plugin.Providers;

public abstract class TitloviSubtitleProvider(string name, params VideoContentType[] contentTypes) : ISubtitleProvider
{
    public string Name => name;

    public IEnumerable<VideoContentType> SupportedMediaTypes => contentTypes;

    public abstract Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken);

    public abstract Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken);

    public static Collection<(string Path, byte[] Buffer)> ExtractSubtitles(byte[] buffer)
    {
        try
        {
            using (var memoryStream = new MemoryStream(buffer))
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
            {
                return new Collection<(string, byte[])>(archive.Entries
                    .Where(entry => entry.Name.EndsWith(".srt", StringComparison.OrdinalIgnoreCase))
                    .Select(entry =>
                    {
                        using var entryStream = entry.Open();
                        using var extractedStream = new MemoryStream();
                        entryStream.CopyTo(extractedStream);
                        return (entry.Name, extractedStream.ToArray());
                    })
                    .ToList());
            }
        }
        catch (Exception)
        {
            return new Collection<(string, byte[])> { (string.Empty, buffer) };
        }
    }

    /// <summary>
    /// Retrieves detailed media information including stream properties for the specified media file.
    /// </summary>
    /// <param name="mediaEncoder">Instance of the <seealso cref="IMediaEncoder"/></param>
    /// <param name="mediaPath">Full path to the media file to analyze.</param>
    /// <param name="cancellationToken">Token to cancel the media analysis operation.</param>
    /// <returns>Media information including codec, resolution, and stream details.</returns>
    /// <remarks>
    /// Currently unused but intended for future enhancement to match subtitles based on
    /// media properties like resolution, codec, and release group.
    /// </remarks>
    protected static async Task<MediaInfo> GetMediaInfoAsync(IMediaEncoder mediaEncoder, string mediaPath, CancellationToken cancellationToken)
    {
        var requestInfo = new MediaInfoRequest
        {
            MediaSource = new MediaSourceInfo
            {
                Id = Guid.NewGuid().ToString(),
                Path = mediaPath,
                Protocol = MediaProtocol.File,
                Container = System.IO.Path.GetExtension(mediaPath).TrimStart('.')
            },
            ExtractChapters = false,
            MediaType = DlnaProfileType.Video
        };
        return await mediaEncoder.GetMediaInfo(requestInfo, cancellationToken).ConfigureAwait(false);
    }
}