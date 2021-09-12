using System;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MusicLibraryCatalogBuilder.Configuration;
using MusicLibraryCatalogBuilder.Entities;
using MusicLibraryCatalogBuilder.Services.Interfaces;

namespace MusicLibraryCatalogBuilder.Services
{
    public class AlbumDirectoryProcessor : IAlbumDirectoryProcessor
    {
        private readonly MusicLibraryConfiguration m_configuration;
        private readonly ILogger<AlbumDirectoryProcessor> m_logger;

        public AlbumDirectoryProcessor(IOptions<MusicLibraryConfiguration> configurationOptions, ILogger<AlbumDirectoryProcessor> logger)
        {
            m_configuration = configurationOptions.Value;
            m_logger = logger;
        }

        public bool CheckIfAlbumDirectory(DirectoryInfo directoryInfo)
        {
            return Regex.IsMatch(directoryInfo.Name, m_configuration.AlbumDirectoryTemplate);
        }

        public AlbumDirectoryInfo ParseAlbumDirectoryInfo(DirectoryInfo directoryInfo)
        {
            try
            {
                Match match = Regex.Match(directoryInfo.Name, m_configuration.AlbumDirectoryTemplate);

                int number = match.Groups.TryGetValue(nameof(AlbumDirectoryInfo.Number), out Group numberGroup) ? int.Parse(numberGroup.Value) : 0;
                string albumName = match.Groups.TryGetValue(nameof(AlbumDirectoryInfo.AlbumName), out Group albumNameGroup) ? albumNameGroup.Value : string.Empty;
                int year = match.Groups.TryGetValue(nameof(AlbumDirectoryInfo.Year), out Group yearGroup) ? int.Parse(yearGroup.Value) : 0;
                string type = match.Groups.TryGetValue(nameof(AlbumDirectoryInfo.Type), out Group typeGroup) ? typeGroup.Value : string.Empty;
                string encoder = match.Groups.TryGetValue(nameof(AlbumDirectoryInfo.Encoder), out Group encoderGroup) ? encoderGroup.Value : string.Empty;
                string format = match.Groups.TryGetValue(nameof(AlbumDirectoryInfo.Format), out Group formatGroup) ? formatGroup.Value : string.Empty;
                string source = match.Groups.TryGetValue(nameof(AlbumDirectoryInfo.Source), out Group sourceGroup) ? sourceGroup.Value : string.Empty;

                return new AlbumDirectoryInfo()
                {
                    Number = number,
                    AlbumName = albumName,
                    Year = year,
                    Type = type,
                    Encoder = encoder,
                    Format = format,
                    Source = source,
                };
            }
            catch (Exception e)
            {
                m_logger.LogError(e, $"Unable to process a directory: {directoryInfo.FullName}.");
                throw;
            }
        }
    }
}
