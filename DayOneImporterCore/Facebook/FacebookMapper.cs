using System.Security.Cryptography;

namespace DayOneImporterCore.Facebook;

public class FacebookMapper : IEntryMapper<Post>
{
    public Entry Map(Post sourceItem, string mediaFolderRoot)
    {
        var entry = new Entry
        {
            CreationDate = BuildCreationDate(sourceItem),
            ModifiedDate = BuildModifiedDate(sourceItem),
            Text = BuildText(sourceItem),
            Location = BuildLocation(sourceItem),
            Tags = BuildTags(sourceItem),
            Photos = BuildPhotos(sourceItem, mediaFolderRoot)
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
            PlaceName = place.Name.FixFacebookEncoding()
        };

        if (place.Coordinate != null)
        {
            location.Latitude = place.Coordinate.Latitude;
            location.Longitude = place.Coordinate.Longitude;
        }

        return location;
    }

    public List<string> BuildTags(Post sourceItem)
    {
        var tags = new List<string>();
        
        tags.AddRange(sourceItem.Tags?.Select(x => x.Name.FixFacebookEncoding()) ?? Array.Empty<string>());

        return tags;
    }

    public List<Photo> BuildPhotos(Post sourceItem, string mediaFolderRoot)
    {
        var photos = new List<Photo>();

        if (sourceItem.Attachments != null)
        {
            foreach (var attachment in sourceItem.Attachments)
            {
                if (attachment.Data != null)
                {
                    foreach (var attachmentDataItem in attachment.Data.Where(x => x.Media != null))
                    {
                        string md5Hash;
                        using (var md5 = MD5.Create())
                        {
                            using (var stream = File.OpenRead(mediaFolderRoot + "/" + attachmentDataItem.Media.Uri))
                            {
                                var hash = md5.ComputeHash(stream);
                                md5Hash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                            }
                        }
                        
                        photos.Add(new Photo
                        {
                            Md5 = md5Hash,
                            SourceLocation = attachmentDataItem.Media.Uri
                        });
                    }
                }
            }
        }
        
        return photos;
    }
}