using MusicLibraryCatalogBuilder.Entities;

namespace MusicLibraryCatalogBuilder.Services.Interfaces
{
    public interface ICatalogInfoProvider
    {
        public CatalogInfo GetCatalogInfo();
    }
}
