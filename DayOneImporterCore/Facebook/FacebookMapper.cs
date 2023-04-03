namespace DayOneImporterCore.Facebook;

public class FacebookMapper : IEntryMapper<Post>
{
    public Entry Map(Post sourceItem)
    {
        var entry = new Entry
        {
            CreationDate = BuildCreationDate(sourceItem),
            ModifiedDate = BuildModifiedDate(sourceItem),
            Text = BuildText(sourceItem),
            Location = BuildLocation(sourceItem)
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

        if (sourceItem.Attachments != null)
        {
            foreach (var attachment in sourceItem.Attachments)
            {
                if (attachment.Data != null)
                {
                    foreach (var attachmentDataItem in attachment.Data.Where(x => x.Text != null))
                    {
                        paragraphs.Add(attachmentDataItem.Text);
                    }

                    foreach (var attachmentDataItem in attachment.Data.Where(x => x.ExternalContext != null))
                    {
                        paragraphs.Add(attachmentDataItem.ExternalContext.Url);
                    }
                }
            }
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

        return string.Join("\n\n", paragraphs).FixFacebookEncoding();
    }

    public Location BuildLocation(Post sourceItem)
    {
        var place = sourceItem.Attachments?
            .SelectMany(x => x.Data)
            .FirstOrDefault(x => x.Place != null)
            ?.Place;

        if (place == null)
        {
            return null;
        }

        var location = new Location
        {
            PlaceName = place.Name
        };

        if (place.Coordinate != null)
        {
            location.Latitude = place.Coordinate.Latitude;
            location.Longitude = place.Coordinate.Longitude;
        }

        return location;
    }
}