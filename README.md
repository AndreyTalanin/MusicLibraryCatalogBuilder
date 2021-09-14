# Music Library Catalog Builder

A tool for creating easy-to-read catalogs for your local music collection.

## Dependencies

This project utilizes the following frameworks, libraries, and technologies:

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) for cross-platform managed code.

## Default Configuration

There are 2 regular expressions configured by default:

Template for artist folders:

    (?'ArtistName'.+)\s\((?'DiscographyFrom'\d{4})-(?'DiscographyTo'\d{4})\)\s\[((?'Genres'[\w\s-]+)(,\s)?)+\]

Examples:

    A Day to Remember (2003-2021) [Pop Punk, Post-Hardcore]
    Story of the Year (2004-2017) [Alternative Rock, Post-Hardcore]

Template for album folders:

    (?'Number'\d{2})\s-\s(?'Type'\w{2,6})\s-\s(?'Year'\d{4})\s-\s(?'AlbumName'.+)\s\[(?'Encoder'\w{1,10}),\s(?'Format'.+),\s(?'Source'CD|Web)\]

Examples:

    01 - LP - 2005 - And Their Name Was Treason [FLAC, 44.1 kHz, 16 bit, CD]
    09 - LP - 2016 - All Our Gods Have Abandoned Us [FLAC, 44.1 kHz, 16 bit, CD]
    04 - EP - 2014 - The Acoustic Things [FLAC, 44.1 kHz, 16 bit, Web]
    02 - Single - 2004 - Anthem of Our Dying Day [FLAC, 44.1 kHz, 16 bit, CD]

These regular expressions are used by default in both `appsettings.json` and `appsettings.Development.json` files. Feel free to change them if you want, just keep the capture group names the same.

Also, please note that the album directories should be placed only under the artist ones, as newly discovered albums will be assigned to the previous artist. This will produce unwanted nesting in the report if the directory structure is different from what is expected.

The root directory is set to `..` as I find it convenient to keep the catalog builder together with the music library, but in a dedicated folder. You can change this as well.
