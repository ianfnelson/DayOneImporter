namespace DayOneImporterCore.Wordpress;

public class WordpressMapper : IEntryMapper<Item>
{
    public Entry Map(Item sourceItem, string mediaFolderRoot)
    {
        var entry = new Entry
        {
            CreationDate = BuildCreationDate(sourceItem),
            ModifiedDate = BuildModifiedDate(sourceItem),
            Text = BuildText(sourceItem)
        };

        return entry;
    }

    private static string BuildText(Item sourceItem)
    {
        return sourceItem.Title;
    }

    private DateTimeOffset BuildModifiedDate(Item sourceItem)
    {
        return DateTimeOffset.Now;
    }

    private DateTimeOffset BuildCreationDate(Item sourceItem)
    {
        return DateTimeOffset.Now;
    }
}