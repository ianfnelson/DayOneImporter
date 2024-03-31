using System.Globalization;
using System.Text;
using Microsoft.Extensions.Primitives;

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
        var sb = new StringBuilder();

        sb.Append(sourceItem.Title);
        sb.Append("\n\n");
        sb.Append(sourceItem.Content);

        return sb.ToString();
    }

    private static DateTimeOffset BuildModifiedDate(Item sourceItem)
    {
        return BuildDate(sourceItem.PostModifiedGmt);
    }

    private static DateTimeOffset BuildCreationDate(Item sourceItem)
    {
        return BuildDate(sourceItem.PostDateGmt);
    }

    private static DateTimeOffset BuildDate(string sourceDate)
    {
        var date = DateTime.ParseExact(sourceDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        return new DateTimeOffset(date);
    }
}