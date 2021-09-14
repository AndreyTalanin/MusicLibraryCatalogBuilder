using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using MusicLibraryCatalogBuilder.Entities;
using MusicLibraryCatalogBuilder.Services.Interfaces;

namespace MusicLibraryCatalogBuilder.Services
{
    public class HtmlDocumentBuilder : IHtmlDocumentBuilder
    {
        public XDocument BuildHmtlDocument(CatalogInfo catalogInfo, IList<ArtistDirectoryInfo> artistDirectoryInfos)
        {
            XElement rootElement = new XElement("html",
                GenerateHead(),
                GenerateBody(catalogInfo, artistDirectoryInfos));
            return new XDocument(rootElement);
        }

        private XElement GenerateHead()
        {
            return new XElement("head",
                new XElement("title", "Music Library Catalog"),
                new XElement("style", "table { border: 2px solid black; } th, td { border: 1px solid black; }"));
        }

        private XElement GenerateBody(CatalogInfo catalogInfo, IList<ArtistDirectoryInfo> artistDirectoryInfos)
        {
            return new XElement("body",
                new XElement("h1", "Music Library Catalog"),
                new XElement("hr"),
                new XElement("h2", "Catalog Info"),
                GenerateCatalogInfo(catalogInfo),
                new XElement("hr"),
                new XElement("h2", "Artists"),
                GenerateArtistsTable(artistDirectoryInfos),
                new XElement("hr"),
                new XElement("h2", "Albums"),
                GenerateAlbumsTable(artistDirectoryInfos),
                new XElement("hr"),
                new XElement("h2", "Credits"),
                GenerateCredits());
        }

        private XElement GenerateCatalogInfo(CatalogInfo catalogInfo)
        {
            return new XElement("div",
                new XElement("p", $"User Name: {catalogInfo.UserName}"),
                new XElement("p", $"Machine Name: {catalogInfo.MachineName}"),
                new XElement("p", $"Generated On: {catalogInfo.GeneratedOn}"));
        }

        private XElement GenerateArtistsTable(IList<ArtistDirectoryInfo> artistDirectoryInfos)
        {
            return new XElement("table",
                new XElement("thead",
                    new XElement("colgroup",
                        new XElement("col"),
                        new XElement("col", new XAttribute("width", "15%")),
                        new XElement("col", new XAttribute("width", "15%")),
                        new XElement("col", new XAttribute("width", "30%"))),
                    new XElement("tr",
                        new XElement("th", "Artist Name", new XAttribute("scope", "col")),
                        new XElement("th", "Discography From", new XAttribute("scope", "col")),
                        new XElement("th", "Discography To", new XAttribute("scope", "col")),
                        new XElement("th", "Genres", new XAttribute("scope", "col")))),
                new XElement("tbody",
                    artistDirectoryInfos.Select(artistDirectoryInfo =>
                    {
                        return new XElement("tr",
                            new XElement("td", artistDirectoryInfo.ArtistName),
                            new XElement("td", artistDirectoryInfo.DiscographyFrom),
                            new XElement("td", artistDirectoryInfo.DiscographyTo),
                            new XElement("td", string.Join(", ", artistDirectoryInfo.Genres)));
                    }).ToList()));
        }

        private XElement GenerateAlbumsTable(IList<ArtistDirectoryInfo> artistDirectoryInfos)
        {
            return new XElement("table",
                new XElement("thead",
                    new XElement("colgroup",
                        new XElement("col", new XAttribute("width", "5%")),
                        new XElement("col", new XAttribute("width", "5%")),
                        new XElement("col"),
                        new XElement("col", new XAttribute("width", "5%")),
                        new XElement("col", new XAttribute("width", "5%")),
                        new XElement("col", new XAttribute("width", "15%")),
                        new XElement("col", new XAttribute("width", "5%"))),
                    new XElement("tr",
                        new XElement("th", "Number", new XAttribute("scope", "col")),
                        new XElement("th", "Year", new XAttribute("scope", "col")),
                        new XElement("th", "Album Name", new XAttribute("scope", "col")),
                        new XElement("th", "Type", new XAttribute("scope", "col")),
                        new XElement("th", "Encoder", new XAttribute("scope", "col")),
                        new XElement("th", "Format", new XAttribute("scope", "col")),
                        new XElement("th", "Source", new XAttribute("scope", "col")))),
                new XElement("tbody",
                    artistDirectoryInfos.Select(artistDirectoryInfo =>
                    {
                        string artistName = artistDirectoryInfo.ArtistName;
                        string discographyFrom = artistDirectoryInfo.DiscographyFrom.ToString();
                        string discographyTo = artistDirectoryInfo.DiscographyTo.ToString();
                        string genres = string.Join(", ", artistDirectoryInfo.Genres);

                        return new
                        {
                            ArtistRow = new XElement("tr",
                                new XElement("td",
                                    $"{artistName} ({discographyFrom}-{discographyTo}) [{genres}]",
                                    new XAttribute("align", "center"),
                                    new XAttribute("colspan", int.MaxValue.ToString()))),
                            AlbumRows = artistDirectoryInfo.Albums.Select(albumDirectoryInfo =>
                            {
                                return new XElement("tr",
                                    new XElement("td", albumDirectoryInfo.Number),
                                    new XElement("td", albumDirectoryInfo.Year),
                                    new XElement("td", albumDirectoryInfo.AlbumName),
                                    new XElement("td", albumDirectoryInfo.Type),
                                    new XElement("td", albumDirectoryInfo.Encoder),
                                    new XElement("td", albumDirectoryInfo.Format),
                                    new XElement("td", albumDirectoryInfo.Source));
                            }).ToList(),
                        };
                    }).Aggregate(new List<XElement>(), (tableData, group) =>
                    {
                        if (group.AlbumRows.Any())
                            tableData.Add(group.ArtistRow);
                        foreach (XElement albumRow in group.AlbumRows)
                            tableData.Add(albumRow);
                        return tableData;
                    })));
        }

        private XElement GenerateCredits()
        {
            return new XElement("p",
                "Generated with" + " ",
                new XElement("a",
                    "AndreyTalanin / MusicLibraryCatalogBuilder (GitHub.com)",
                    new XAttribute("href", "https://github.com/AndreyTalanin/MusicLibraryCatalogBuilder/"),
                    new XAttribute("target", "_blank")),
                ".");
        }
    }
}
