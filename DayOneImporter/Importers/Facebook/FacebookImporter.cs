using System.Text.Json;
using DayOneImporter.Importers.Facebook.Model;
using Microsoft.Extensions.Logging;

namespace DayOneImporter.Importers.Facebook;

public class FacebookImporter(ILogger<ImporterBase<Post>> logger, IEntryMapper<Post> entryMapper)
    : ImporterBase<Post>(logger, entryMapper)
{
    protected override string SourceSystemName => "Facebook";

    protected override string MediaFolderRoot => "/Users/ian/dev/Facebook/";

    protected override IList<Post> LoadSourceItems()
    {
        using var openStream = File.OpenRead("/Users/ian/dev/Facebook/posts/your_posts_1.json");

        var posts = JsonSerializer.Deserialize<List<Post>>(openStream);
        
        return posts ?? throw new InvalidOperationException("Could not deserialize Facebook file to list of Posts");
    }

    protected override IList<Post> FilterSourceItems(IList<Post> sourceItems)
    {
        return sourceItems;
    }
}