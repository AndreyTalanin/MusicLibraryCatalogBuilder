using System;

namespace MusicLibraryCatalogBuilder.Entities
{
    public class CatalogInfo
    {
        public string UserName { get; set; } = string.Empty;

        public string MachineName { get; set; } = string.Empty;

        public DateTimeOffset GeneratedOn { get; set; }
    }
}
