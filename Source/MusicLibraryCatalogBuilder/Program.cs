using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace MusicLibraryCatalogBuilder
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args);
        }
    }
}
