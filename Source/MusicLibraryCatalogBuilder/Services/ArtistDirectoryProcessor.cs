using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MusicLibraryCatalogBuilder.Configuration;
using MusicLibraryCatalogBuilder.Entities;
using MusicLibraryCatalogBuilder.Services.Interfaces;

namespace MusicLibraryCatalogBuilder.Services
{
    public class ArtistDirectoryProcessor : IArtistDirectoryProcessor
    {
        private readonly MusicLibraryConfiguration m_configuration;
        private readonly ILogger<ArtistDirectoryProcessor> m_logger;

        public ArtistDirectoryProcessor(IOptions<MusicLibraryConfiguration> configurationOptions, ILogger<ArtistDirectoryProcessor> logger)
        {
            m_configuration = configurationOptions.Value;
            m_logger = logger;
        }

        public bool CheckIfArtistDirectory(DirectoryInfo directoryInfo)
        {
            return Regex.IsMatch(directoryInfo.Name, m_configuration.ArtistDirectoryTemplate);
        }

        public ArtistDirectoryInfo ParseArtistDirectoryInfo(DirectoryInfo directoryInfo)
        {
            try
            {
                Match match = Regex.Match(directoryInfo.Name, m_configuration.ArtistDirectoryTemplate);

                string artistName = match.Groups.TryGetValue(nameof(ArtistDirectoryInfo.ArtistName), out Group artistNameGroup) ? artistNameGroup.Value : string.Empty;
                int discographyFrom = match.Groups.TryGetValue(nameof(ArtistDirectoryInfo.DiscographyFrom), out Group discographyFromGroup) ? int.Parse(discographyFromGroup.Value) : 0;
                int discographyTo = match.Groups.TryGetValue(nameof(ArtistDirectoryInfo.DiscographyTo), out Group discographyToGroup) ? int.Parse(discographyToGroup.Value) : 0;
                IList<string> genres = match.Groups.TryGetValue(nameof(ArtistDirectoryInfo.Genres), out Group genresGroup) ? genresGroup.Captures.Select(capture => capture.Value).ToList() : new List<string>();

                return new ArtistDirectoryInfo()
                {
                    ArtistName = artistName,
                    DiscographyFrom = discographyFrom,
                    DiscographyTo = discographyTo,
                    Genres = genres,
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
