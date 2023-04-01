using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DayOneImporterCore.Facebook;

public class FacebookImporter : ImporterBase<Post>
{
    public FacebookImporter(ILogger<ImporterBase<Post>> logger, IEntryMapper<Post> entryMapper) : base(logger, entryMapper)
    {
    }

    public override string SourceSystemName => "Facebook";

    protected override IList<Post> LoadSourceItems()
    {
        using FileStream openStream = File.OpenRead("/Users/ian/dev/Facebook/posts/your_posts_1.json");

        return JsonSerializer.Deserialize<List<Post>>(openStream);
    }

    protected override IList<Post> FilterSourceItems(IList<Post> sourceItems)
    {
        return sourceItems
            .Where(x => x.Data != null)
            .Where(x => x.Data.Any(y => !string.IsNullOrEmpty(y.Post)))
            .ToList();
    }
}