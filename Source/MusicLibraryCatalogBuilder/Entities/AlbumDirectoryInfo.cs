namespace MusicLibraryCatalogBuilder.Entities
{
    public class AlbumDirectoryInfo
    {
        public int Number { get; set; }

        public string AlbumName { get; set; } = string.Empty;

        public int Year { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Encoder { get; set; } = string.Empty;

        public string Format { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;
    }
}
