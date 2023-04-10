using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DayOneImporterCore.Twitter;

public class TwitterImporter : ImporterBase<Tweet>
{
    public TwitterImporter(ILogger<ImporterBase<Tweet>> logger, IEntryMapper<Tweet> entryMapper) : base(logger, entryMapper)
    {
    }

    public override int BatchSize => 1000;

    public override string SourceSystemName => "Twitter";
    public override string MediaFolderRoot => "/Users/ian/dev/Twitter/";
    protected override IList<Tweet> LoadSourceItems()
    {
        using FileStream openStream = File.OpenRead("/Users/ian/dev/Twitter/data/tweets.json");

        var records = JsonSerializer.Deserialize<List<Record>>(openStream);

        return records.Select(x => x.Tweet).ToList();
    }

    protected override IList<Tweet> FilterSourceItems(IList<Tweet> sourceItems)
    {
        return sourceItems;
    }
}