using System.Text;
using DayOneImporter.Importers.BookLog.Model;
using Microsoft.Extensions.Logging;
using TinyCsvParser;

namespace DayOneImporter.Importers.BookLog;

public class BookLogImporter(ILogger<ImporterBase<Book>> logger, IEntryMapper<Book> entryMapper)
    : ImporterBase<Book>(logger, entryMapper)
{
    protected override string SourceSystemName => "Book Log";
    protected override string MediaFolderRoot => null;
    protected override IList<Book> LoadSourceItems()
    {
        var options = new CsvParserOptions(true, ',');
        var mapper = new CsvBookMapping();
        var parser = new CsvParser<Book>(options, mapper);

        var books = parser.ReadFromFile("/Users/ian/dev/Book Log.csv", Encoding.UTF8).ToList();

        return books.Select(x => x.Result).ToList();
    }

    protected override IList<Book> FilterSourceItems(IList<Book> sourceItems)
    {
        return sourceItems;
    }
}