using System.Collections.Generic;
using System.Xml.Linq;

using MusicLibraryCatalogBuilder.Entities;

namespace MusicLibraryCatalogBuilder.Services.Interfaces
{
    public interface IHtmlDocumentBuilder
    {
        public XDocument BuildHmtlDocument(CatalogInfo catalogInfo, IList<ArtistDirectoryInfo> artistDirectoryInfos);
    }
}
