using System.Text.Json;
using DayOneImporter.Importers.Twitter.Model;
using Microsoft.Extensions.Logging;

namespace DayOneImporter.Importers.Twitter;

public class TwitterImporter(ILogger<ImporterBase<Tweet>> logger, IEntryMapper<Tweet> entryMapper)
    : ImporterBase<Tweet>(logger, entryMapper)
{
    protected override int BatchSize => 1000;

    protected override string SourceSystemName => "Twitter";
    protected override string MediaFolderRoot => "/Users/ian/dev/Twitter/data/tweets_media/";
    protected override IList<Tweet> LoadSourceItems()
    {
        using FileStream openStream = File.OpenRead("/Users/ian/dev/Twitter/data/tweets.json");

        var records = JsonSerializer.Deserialize<List<Record>>(openStream);

        return records.Select(x => x.Tweet).ToList();
    }

    protected override IList<Tweet> FilterSourceItems(IList<Tweet> sourceItems)
    {
        return sourceItems.Where(x => !x.FullText.StartsWith("RT ")).ToList();
    }
}