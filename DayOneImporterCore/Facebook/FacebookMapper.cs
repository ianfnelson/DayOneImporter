namespace DayOneImporterCore.Facebook;

public class FacebookMapper : IEntryMapper<Post>
{
    public Entry Map(Post sourceItem)
    {
        var entry = new Entry
        {
            CreationDate = DateTimeOffset.FromUnixTimeSeconds(sourceItem.Timestamp)
        };

        foreach (var postDataItem in sourceItem.Data)
        {
            if (!string.IsNullOrEmpty(postDataItem.Post))
            {
                entry.Text = postDataItem.Post;
            }
        }

        return entry;
    }
}