using System.Collections.Generic;

namespace MusicLibraryCatalogBuilder.Configuration
{
    public class MusicLibraryConfiguration
    {
        public string RootDirectory { get; set; } = string.Empty;

        public IList<string> IncludedDirectories { get; set; } = new List<string>();

        public IList<string> ExcludedDirectories { get; set; } = new List<string>();

        public string ArtistDirectoryTemplate { get; set; } = string.Empty;

        public string AlbumDirectoryTemplate { get; set; } = string.Empty;
    }
}
