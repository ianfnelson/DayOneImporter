namespace DayOneImporterCore.Facebook;

public class FacebookMapper : IEntryMapper<Post>
{
    public Entry Map(Post sourceItem)
    {
        var entry = new Entry
        {
            CreationDate = BuildCreationDate(sourceItem),
            ModifiedDate = BuildModifiedDate(sourceItem),
            Text = BuildText(sourceItem)
        };

        return entry;
    }

    public DateTimeOffset BuildCreationDate(Post sourceItem)
    {
        return DateTimeOffset.FromUnixTimeSeconds(sourceItem.Timestamp);
    }

    public DateTimeOffset BuildModifiedDate(Post sourceItem)
    {
        var updateTimestamp = sourceItem.Data?.FirstOrDefault(
            x => x.UpdateTimestamp != null)?.UpdateTimestamp ?? sourceItem.Timestamp;

        return DateTimeOffset.FromUnixTimeSeconds(updateTimestamp);
    }

    public string BuildText(Post sourceItem)
    {
        var paragraphs = new List<string>();

        if (!string.IsNullOrWhiteSpace(sourceItem.Title))
        {
            paragraphs.Add(sourceItem.Title);
        }

        if (sourceItem.Data != null)
        {
            foreach (var postDataItem in sourceItem.Data)
            {
                if (!string.IsNullOrWhiteSpace(postDataItem.Post))
                {
                    paragraphs.Add(postDataItem.Post);
                }
            }
        }

        return string.Join("\n\n", paragraphs);
        }
    }
    