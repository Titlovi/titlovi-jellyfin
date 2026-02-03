using MediaBrowser.Controller.Subtitles;
using Microsoft.Extensions.DependencyInjection;
using Titlovi.Plugin.Providers;

namespace Titlovi.Plugin.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to register Titlovi subtitle providers.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Titlovi subtitle providers for movies and episodes.
    /// </summary>
    /// <param name="services">Service collection to register providers with.</param>
    /// <returns>Service collection for chaining.</returns>
    public static IServiceCollection AddTitloviSubtitleProviders(this IServiceCollection services) => services
        .AddSingleton<ISubtitleProvider, TitloviMovieSubtitleProvider>()
        .AddSingleton<ISubtitleProvider, TitloviEpisodeSubtitleProvider>();
}