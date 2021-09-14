using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MusicLibraryCatalogBuilder.Configuration;
using MusicLibraryCatalogBuilder.Services;
using MusicLibraryCatalogBuilder.Services.Hosted;
using MusicLibraryCatalogBuilder.Services.Interfaces;

namespace MusicLibraryCatalogBuilder
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).ConfigureServices(ConfigureServices).Build();
            await host.RunAsync();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<MusicLibraryConfiguration>(context.Configuration.GetSection(nameof(MusicLibraryConfiguration)));

            services.AddSingleton<IArtistDirectoryProcessor, ArtistDirectoryProcessor>();
            services.AddSingleton<IAlbumDirectoryProcessor, AlbumDirectoryProcessor>();
            services.AddSingleton<ICatalogInfoProvider, CatalogInfoProvider>();
            services.AddSingleton<IHtmlDocumentBuilder, HtmlDocumentBuilder>();

            services.AddHostedService<MusicLibraryProcessor>();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args);
        }
    }
}
