using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MusicLibraryCatalogBuilder.Configuration;

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
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args);
        }
    }
}
