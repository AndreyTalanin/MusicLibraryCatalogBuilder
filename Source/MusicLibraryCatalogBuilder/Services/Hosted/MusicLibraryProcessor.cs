using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MusicLibraryCatalogBuilder.Configuration;
using MusicLibraryCatalogBuilder.Entities;
using MusicLibraryCatalogBuilder.Services.Interfaces;

namespace MusicLibraryCatalogBuilder.Services.Hosted
{
    public class MusicLibraryProcessor : BackgroundService
    {
        private readonly IServiceProvider m_serviceProvider;
        private readonly IHostApplicationLifetime m_hostApplicationLifetime;
        private readonly IArtistDirectoryProcessor m_artistDirectoryProcessor;
        private readonly IAlbumDirectoryProcessor m_albumDirectoryProcessor;
        private readonly ICatalogInfoProvider m_catalogInfoProvider;
        private readonly IHtmlDocumentBuilder m_htmlDocumentBuilder;
        private readonly MusicLibraryConfiguration m_configuration;
        private readonly ILogger<MusicLibraryProcessor> m_logger;

        public MusicLibraryProcessor(
            IServiceProvider serviceProvider,
            IHostApplicationLifetime hostApplicationLifetime,
            IArtistDirectoryProcessor artistDirectoryProcessor,
            IAlbumDirectoryProcessor albumDirectoryProcessor,
            ICatalogInfoProvider catalogInfoProvider,
            IHtmlDocumentBuilder htmlDocumentBuilder,
            IOptions<MusicLibraryConfiguration> configurationOptions,
            ILogger<MusicLibraryProcessor> logger
            )
        {
            m_serviceProvider = serviceProvider;
            m_hostApplicationLifetime = hostApplicationLifetime;
            m_artistDirectoryProcessor = artistDirectoryProcessor;
            m_albumDirectoryProcessor = albumDirectoryProcessor;
            m_catalogInfoProvider = catalogInfoProvider;
            m_htmlDocumentBuilder = htmlDocumentBuilder;
            m_configuration = configurationOptions.Value;
            m_logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope serviceScope = m_serviceProvider.CreateScope();

            HashSet<string> includedDirectories = m_configuration.IncludedDirectories.Select(directory => Path.Combine(m_configuration.RootDirectory, directory)).ToHashSet();
            HashSet<string> excludedDirectories = m_configuration.ExcludedDirectories.Select(directory => Path.Combine(m_configuration.RootDirectory, directory)).ToHashSet();

            void ProcessDirectory(DirectoryInfo directoryInfo, List<ArtistDirectoryInfo> artistDirectoryInfos, bool skipExcludedDirectory)
            {
                m_logger.LogInformation($"Processing the '{directoryInfo.Name}' directory.");

                if (skipExcludedDirectory && includedDirectories.Contains(directoryInfo.FullName))
                    m_logger.LogInformation("The directory was explicitly included, continuing with processing.");
                else if (skipExcludedDirectory)
                {
                    m_logger.LogInformation("The directory was explicitly excluded, skipping it.");
                    return;
                }

                if (m_artistDirectoryProcessor.CheckIfArtistDirectory(directoryInfo))
                {
                    artistDirectoryInfos.Add(m_artistDirectoryProcessor.ParseArtistDirectoryInfo(directoryInfo));
                    m_logger.LogInformation($"The '{directoryInfo.Name}' directory is an artist directory, adding it to the list.");
                }
                else if (m_albumDirectoryProcessor.CheckIfAlbumDirectory(directoryInfo))
                {
                    if (artistDirectoryInfos.Count > 0)
                    {
                        ArtistDirectoryInfo artistDirectoryInfo = artistDirectoryInfos.Last();
                        artistDirectoryInfo.Albums.Add(m_albumDirectoryProcessor.ParseAlbumDirectoryInfo(directoryInfo));
                        m_logger.LogInformation($"The '{directoryInfo.Name}' directory is an album directory, adding it to the list of albums.");
                    }
                    else
                        m_logger.LogError($"The '{directoryInfo.Name}' directory is an album directory. Check the directory structure.");
                }

                foreach (DirectoryInfo nestedDirectoryInfo in directoryInfo.GetDirectories())
                    ProcessDirectory(nestedDirectoryInfo, artistDirectoryInfos, excludedDirectories.Contains(nestedDirectoryInfo.FullName));
            }

            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(m_configuration.RootDirectory);

            List<ArtistDirectoryInfo> artistDirectoryInfos = new List<ArtistDirectoryInfo>();
            ProcessDirectory(rootDirectoryInfo, artistDirectoryInfos, false);

            CatalogInfo catalogInfo = m_catalogInfoProvider.GetCatalogInfo();
            using (StreamWriter streamWriter = new StreamWriter(m_configuration.OutputFile))
            {
                using XmlWriter xmlWriter = XmlWriter.Create(streamWriter, new XmlWriterSettings() { Async = true, OmitXmlDeclaration = true, Indent = m_configuration.WriteIndented });
                await m_htmlDocumentBuilder.BuildHmtlDocument(catalogInfo, artistDirectoryInfos).WriteToAsync(xmlWriter, stoppingToken);
            }

            m_hostApplicationLifetime.StopApplication();
        }
    }
}
