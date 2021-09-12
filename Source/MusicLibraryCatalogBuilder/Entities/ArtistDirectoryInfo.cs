using System.Collections.Generic;

namespace MusicLibraryCatalogBuilder.Entities
{
    public class ArtistDirectoryInfo
    {
        public string ArtistName { get; set; } = string.Empty;

        public int DiscographyFrom { get; set; }

        public int DiscographyTo { get; set; }

        public IList<string> Genres { get; set; } = new List<string>();

        public IList<AlbumDirectoryInfo> Albums { get; set; } = new List<AlbumDirectoryInfo>();
    }
}
