using System.IO;

using MusicLibraryCatalogBuilder.Entities;

namespace MusicLibraryCatalogBuilder.Services.Interfaces
{
    public interface IAlbumDirectoryProcessor
    {
        public bool CheckIfAlbumDirectory(DirectoryInfo directoryInfo);

        public AlbumDirectoryInfo ParseAlbumDirectoryInfo(DirectoryInfo directoryInfo);
    }
}
