using DayOneImporter.Importers.BookLog.Model;
using TinyCsvParser.Mapping;

namespace DayOneImporter.Importers.BookLog;

public class CsvBookMapping : CsvMapping<Book>
{
    public CsvBookMapping()
    {
        MapProperty(0, x => x.Date);
        MapProperty(1, x => x.Title);
        MapProperty(2, x => x.Kind);
    }
}