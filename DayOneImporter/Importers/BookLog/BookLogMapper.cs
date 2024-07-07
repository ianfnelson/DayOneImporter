using DayOneImporter.Importers.BookLog.Model;
using DayOneImporter.Model;

namespace DayOneImporter.Importers.BookLog;

public class BookLogMapper : IEntryMapper<Book>
{
    public Entry Map(Book sourceItem, string mediaFolderRoot)
    {
        var entryDate = BuildEntryDate(sourceItem);
        
        return new Entry
        {
            CreationDate = entryDate,
            ModifiedDate = entryDate,
            IsAllDay = true,
            Text = BuildText(sourceItem)
        };
    }

    private static DateTimeOffset BuildEntryDate(Book sourceItem)
    {
        return new DateTimeOffset(sourceItem.Date);
    }

    private string BuildText(Book sourceItem)
    {
        return $"{sourceItem.Title}\n\n{sourceItem.Kind}";
    }
}