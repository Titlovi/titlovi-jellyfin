using MediaBrowser.Controller.Subtitles;
using Microsoft.Extensions.DependencyInjection;
using Titlovi.Plugin.Providers;

namespace Titlovi.Plugin.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTitloviSubtitleProviders(this IServiceCollection services) => services
        .AddSingleton<ISubtitleProvider, TitloviMovieSubtitleProvider>()
        .AddSingleton<ISubtitleProvider, TitloviEpisodeSubtitleProvider>();
}