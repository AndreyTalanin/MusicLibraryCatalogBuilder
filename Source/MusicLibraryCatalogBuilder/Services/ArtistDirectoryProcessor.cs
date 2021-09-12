using System;
using System.IO;

using MusicLibraryCatalogBuilder.Entities;
using MusicLibraryCatalogBuilder.Services.Interfaces;

namespace MusicLibraryCatalogBuilder.Services
{
    public class ArtistDirectoryProcessor : IArtistDirectoryProcessor
    {
        public bool CheckIfArtistDirectory(DirectoryInfo directoryInfo) => throw new NotImplementedException();

        public ArtistDirectoryInfo ParseArtistDirectoryInfo(DirectoryInfo directoryInfo) => throw new NotImplementedException();
    }
}
