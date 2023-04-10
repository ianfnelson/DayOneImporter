using System.Globalization;

namespace DayOneImporterCore.Twitter;

public class TwitterMapper : IEntryMapper<Tweet>
{
    public Entry Map(Tweet sourceItem, string mediaFolderRoot)
    {
        var tweetDate = BuildTweetDate(sourceItem);
            
        var entry = new Entry
        {
            CreationDate = tweetDate,
            ModifiedDate = tweetDate,
            Text = BuildText(sourceItem)
        };

        return entry;
    }

    public DateTimeOffset BuildTweetDate(Tweet sourceItem)
    {
        var date = DateTime.ParseExact(sourceItem.CreatedAt, "ddd MMM dd HH:mm:ss +ffff yyyy", CultureInfo.InvariantCulture);

        return new DateTimeOffset(date);
    }

    public string BuildText(Tweet sourceItem)
    {
        return sourceItem.FullText;
    }
}