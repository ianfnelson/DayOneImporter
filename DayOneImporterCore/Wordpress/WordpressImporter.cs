using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace DayOneImporterCore.Wordpress;

public class WordpressImporter : ImporterBase<Item>
{
    public WordpressImporter(ILogger<ImporterBase<Item>> logger, IEntryMapper<Item> entryMapper) : base(logger, entryMapper)
    {
    }

    public override string SourceSystemName => "WordPress";
    public override string MediaFolderRoot => "/Users/ian/dev/WordPress/media/";
    protected override IList<Item> LoadSourceItems()
    {
        using var openStream = File.OpenRead("/Users/ian/dev/WordPress/wordpress.xml");
        
        var ser = new XmlSerializer(typeof(Rss));

        var rss = (Rss)ser.Deserialize(openStream);

        return rss.Channel.Items;
    }

    protected override IList<Item> FilterSourceItems(IList<Item> sourceItems)
    {
        return sourceItems;
    }
}