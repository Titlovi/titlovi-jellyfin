using FuzzySharp;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.MediaInfo;
using System.Globalization;
using Titlovi.Api.Models;

namespace Titlovi.Plugin.Extensions;

/// <summary>
/// Extension methods for <seealso cref="MediaInfo"/> to enhance subtitle matching.
/// </summary>
public static class MediaInfoExtensions
{
    /// <summary>
    /// Calculates a fuzzy matching score between media file properties and subtitle release information.
    /// </summary>
    /// <param name="mediaInfo">Media information containing stream properties.</param>
    /// <param name="subtitle">Subtitle with release information to match against.</param>
    /// <returns>Score indicating how well the subtitle matches the media file properties.</returns>
    public static float HashScore(this MediaInfo mediaInfo, Subtitle subtitle)
    {
        ArgumentNullException.ThrowIfNull(mediaInfo);
        ArgumentNullException.ThrowIfNull(subtitle);

        var release = subtitle.Release;
        if (string.IsNullOrEmpty(release))
            return 0f;
        release = release.ToLowerInvariant();

        int hashScore = 0;
        foreach (var stream in mediaInfo.MediaStreams)
        {
            switch (stream.Type)
            {
                case MediaStreamType.Video:
                    hashScore += Fuzz.PartialRatio(stream.Codec.ToLowerInvariant(), release);

                    var height = Convert.ToString(stream.Height, CultureInfo.InvariantCulture);
                    if (height is not null)
                        hashScore += Fuzz.PartialRatio(height, release);
                    break;
                case MediaStreamType.Audio:
                    hashScore += Fuzz.PartialRatio(stream.Codec.ToLowerInvariant(), release);
                    break;
            }
        }
        return hashScore;
    }
}