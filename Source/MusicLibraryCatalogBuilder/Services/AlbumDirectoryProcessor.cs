using System;
using System.IO;

using MusicLibraryCatalogBuilder.Entities;
using MusicLibraryCatalogBuilder.Services.Interfaces;

namespace MusicLibraryCatalogBuilder.Services
{
    public class AlbumDirectoryProcessor : IAlbumDirectoryProcessor
    {
        public bool CheckIfAlbumDirectory(DirectoryInfo directoryInfo) => throw new NotImplementedException();

        public AlbumDirectoryInfo ParseAlbumDirectoryInfo(DirectoryInfo directoryInfo) => throw new NotImplementedException();
    }
}
