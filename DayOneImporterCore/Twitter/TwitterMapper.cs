using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DayOneImporterCore.Twitter;

public class TwitterMapper : IEntryMapper<Tweet>
{
    public Entry Map(Tweet sourceItem, string mediaFolderRoot)
    {
        var tweetDate = BuildTweetDate(sourceItem);

        var media = BuildMedia(sourceItem, mediaFolderRoot);
        
        var entry = new Entry
        {
            CreationDate = tweetDate,
            ModifiedDate = tweetDate,
            Text = BuildText(sourceItem),
            Photos = media.Where(x => x.Type!= "mp4").ToList(),
            Videos = media.Where(x => x.Type=="mp4").ToList()
        };

        return entry;
    }

    private List<Media> BuildMedia(Tweet sourceItem, string mediaFolderRoot)
    {
        var output = new List<Media>();
        
        if (sourceItem.ExtendedEntities != null && sourceItem.ExtendedEntities.Media != null && sourceItem.ExtendedEntities.Media.Any())
        {
            foreach (var item in sourceItem.ExtendedEntities.Media)
            {
                string filename;
                if (item.VideoInfo !=null)
                {
                    filename = new DirectoryInfo(mediaFolderRoot).GetFiles().Single(x => x.Name.Contains(sourceItem.Id)).Name;
                }
                else
                {
                    filename = sourceItem.Id + "-" + item.MediaUrl.Substring(item.MediaUrl.LastIndexOf("/") + 1);
                }

                var sourceLocation = filename;

                string md5Hash;
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(mediaFolderRoot + filename))
                    {
                        var hash = md5.ComputeHash(stream);
                        md5Hash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }

                string type;
                if (sourceLocation.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    type = "jpeg";
                }
                else if (sourceLocation.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    type = "png";
                }
                else if (sourceLocation.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    type = "mp4";
                }
                else
                {
                    throw new InvalidOperationException("Unknown file type - " + sourceLocation);
                }
                
                output.Add(new Media
                {
                    Md5 = md5Hash,
                    SourceLocation = sourceLocation,
                    Type = type
                });
            }
        }

        return output;
    }

    public DateTimeOffset BuildTweetDate(Tweet sourceItem)
    {
        var date = DateTime.ParseExact(sourceItem.CreatedAt, "ddd MMM dd HH:mm:ss +ffff yyyy", CultureInfo.InvariantCulture);

        return new DateTimeOffset(date);
    }

    public string BuildText(Tweet sourceItem)
    {
        var sb = new StringBuilder(sourceItem.FullText);

        if (sourceItem.Entities?.Urls != null && sourceItem.Entities.Urls.Any())
        {
            foreach (var url in sourceItem.Entities.Urls)
            {
                sb = sb.Replace(url.IncludedUrl, url.ExpandedUrl);
            }
        }

        if (sourceItem.ExtendedEntities?.Media != null && sourceItem.ExtendedEntities.Media.Any())
        {
            foreach (var media in sourceItem.ExtendedEntities.Media)
            {
                sb = sb.Replace(media.Url, string.Empty);
            }
        }

        sb.Append("\n\n");

        sb.Append("https://twitter.com/ianfnelson/status/" + sourceItem.Id);
        
        return sb.ToString();
    }
}