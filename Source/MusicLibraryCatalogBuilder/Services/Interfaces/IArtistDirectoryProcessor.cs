using System.IO;

using MusicLibraryCatalogBuilder.Entities;

namespace MusicLibraryCatalogBuilder.Services.Interfaces
{
    public interface IArtistDirectoryProcessor
    {
        public bool CheckIfArtistDirectory(DirectoryInfo directoryInfo);

        public ArtistDirectoryInfo ParseArtistDirectoryInfo(DirectoryInfo directoryInfo);
    }
}
