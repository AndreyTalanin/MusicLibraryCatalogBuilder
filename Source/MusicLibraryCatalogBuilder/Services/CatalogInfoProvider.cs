using System;

using MusicLibraryCatalogBuilder.Entities;
using MusicLibraryCatalogBuilder.Services.Interfaces;

namespace MusicLibraryCatalogBuilder.Services
{
    public class CatalogInfoProvider : ICatalogInfoProvider
    {
        public CatalogInfo GetCatalogInfo()
        {
            return new CatalogInfo()
            {
                UserName = $"{Environment.UserDomainName}\\{Environment.UserName}",
                MachineName = Environment.MachineName,
                GeneratedOn = DateTimeOffset.Now,
            };
        }
    }
}
