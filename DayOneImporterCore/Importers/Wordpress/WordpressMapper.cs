using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DayOneImporterCore.Importers.Wordpress.Model;
using DayOneImporterCore.Model;
using Html2Markdown;

namespace DayOneImporterCore.Importers.Wordpress;

public class WordpressMapper : IEntryMapper<Item>
{
    public Entry Map(Item sourceItem, string mediaFolderRoot)
    {
        var text = ConvertText(sourceItem);

        var photos = BuildPhotos(text, mediaFolderRoot);
        
        var entry = new Entry
        {
            CreationDate = BuildCreationDate(sourceItem),
            ModifiedDate = BuildModifiedDate(sourceItem),
            Tags = BuildTags(sourceItem),
            Text = text,
            Photos = photos
        };

        return entry;
    }
    
    private static string ConvertText(Item sourceItem)
    {
        var content = sourceItem.Content
            .Replace("<!--kg-card-begin: html-->", "")
            .Replace("<!--kg-card-end: html-->", "")
            .Replace("<figure class=\"kg-card kg-image-card\">", "")
            .Replace("</figure>", "");
        
        var sb = new StringBuilder();
        sb.Append(sourceItem.Title);
        sb.Append("\n\n");
        sb.Append(new Converter().Convert(content));

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

    private static List<Media> BuildPhotos(string text, string mediaFolderRoot)
    {
        var photos = new List<Media>();

        var urls = GetImageUrls(text).ToList();

        foreach (var url in urls)
        {
            var md5OfUrl = GetMd5OfUrl(url);
            var suffix = url.Substring(url.LastIndexOf("."));

            var localFile = Path.Combine(mediaFolderRoot, md5OfUrl + suffix);
            if (!File.Exists(localFile))
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = httpClient.GetAsync(url).Result)
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            continue;
                        }

                        using (FileStream fs = new FileStream(localFile, FileMode.Create))
                        {
                            response.Content.ReadAsStream().CopyTo(fs);
                        }
                    }
                }
            }

            var md5OfFile = GetMd5OfFile(localFile);
            
            string type;
            if (localFile.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                localFile.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                type = "jpeg";
            }
            else if (localFile.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                type = "png";
            }
            else if (localFile.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                type = "gif";
            }
            else
            {
                throw new InvalidOperationException("Unknown file type - " + localFile);
            }
            
            photos.Add(new Media
            {
                Md5 = md5OfFile,
                SourceLocation = md5OfUrl + suffix,
                Type =type
            });
        }
        
        return photos;
    }

    private static string GetMd5OfUrl(string url)
    {
        using var md5 = MD5.Create();
        
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(url));
        return BitConverter.ToString(hash).Replace("-", "");
    }

    private static string GetMd5OfFile(string filePath)
    {
        using var md5 = MD5.Create();
        using var fs = File.OpenRead(filePath);
        
        var hash = md5.ComputeHash(fs);
        return BitConverter.ToString(hash).Replace("-", "");
    }

    private static IEnumerable<string> GetImageUrls(string text)
    {
        var urls = Regex.Matches(text, @"!\[[^\]]*\]\((https://blog\.iannelson\.uk[^)]+)\)");

        foreach (Match match in urls)
        {
            yield return match.Groups[1].Value.Split(" ")[0];
        }
    }

    private static List<string> BuildTags(Item sourceItem)
    {
        var tags = new List<string>();
        
        tags.AddRange(sourceItem.Categories?.Where(x => x.Domain.Equals("category")).Select(x => x.Value) ?? Array.Empty<string>());

        return tags;
    }
}