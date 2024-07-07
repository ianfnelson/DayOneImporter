using System.Text;
using DayOneImporter.Importers.Albums.Model;
using DayOneImporter.Importers.BookLog;
using DayOneImporter.Importers.BookLog.Model;
using Microsoft.Extensions.Logging;
using TinyCsvParser;

namespace DayOneImporter.Importers.Albums;

public class AlbumImporter(ILogger<ImporterBase<Album>> logger, IEntryMapper<Album> entryMapper)
    : ImporterBase<Album>(logger, entryMapper)
{
    protected override string SourceSystemName => "Album Log";
    protected override string MediaFolderRoot => null;
    
    protected override IList<Album> LoadSourceItems()
    {
        var options = new CsvParserOptions(true, ',');
        var mapper = new CsvAlbumMapping();
        var parser = new CsvParser<Album>(options, mapper);

        var albums = parser.ReadFromFile("/Users/ian/dev/Albums.csv", Encoding.UTF8).ToList();

        return albums.Select(x => x.Result).ToList();
    }

    protected override IList<Album> FilterSourceItems(IList<Album> sourceItems)
    {
        return sourceItems;
    }
}