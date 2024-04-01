using System.Security.Cryptography;
using DayOneImporterCore.Importers.Facebook.Model;
using DayOneImporterCore.Model;

namespace DayOneImporterCore.Importers.Facebook;

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

    public static DateTimeOffset BuildCreationDate(Post sourceItem)
    {
        return DateTimeOffset.FromUnixTimeSeconds(sourceItem.Timestamp);
    }

    public static DateTimeOffset BuildModifiedDate(Post sourceItem)
    {
        var updateTimestamp = sourceItem.Data?.FirstOrDefault(
            x => x.UpdateTimestamp != null)?.UpdateTimestamp ?? sourceItem.Timestamp;

        return DateTimeOffset.FromUnixTimeSeconds(updateTimestamp);
    }

    public static string BuildText(Post sourceItem)
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

    public static Location BuildLocation(Post sourceItem)
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

    public static List<string> BuildTags(Post sourceItem)
    {
        var tags = new List<string>();
        
        tags.AddRange(sourceItem.Tags?.Select(x => x.Name.FixFacebookEncoding()) ?? Array.Empty<string>());

        return tags;
    }

    public static List<Media> BuildPhotos(Post sourceItem, string mediaFolderRoot)
    {
        var photos = new List<Media>();

        if (sourceItem.Attachments != null)
        {
            foreach (var attachment in sourceItem.Attachments)
            {
                if (attachment.Data != null)
                {
                    foreach (var attachmentDataItem in attachment.Data.Where(x => x.FbMedia != null))
                    {
                        string md5Hash;
                        using (var md5 = MD5.Create())
                        {
                            using (var stream = File.OpenRead(mediaFolderRoot + "/" + attachmentDataItem.FbMedia.Uri))
                            {
                                var hash = md5.ComputeHash(stream);
                                md5Hash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                            }
                        }
                        
                        photos.Add(new Media
                        {
                            Md5 = md5Hash,
                            SourceLocation = attachmentDataItem.FbMedia.Uri,
                            Type = "jpeg"
                        });
                    }
                }
            }
        }
        
        return photos;
    }
}