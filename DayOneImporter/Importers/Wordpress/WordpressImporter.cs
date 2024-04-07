using System.Xml.Serialization;
using DayOneImporter.Importers.Wordpress.Model;
using Microsoft.Extensions.Logging;

namespace DayOneImporter.Importers.Wordpress;

public class WordpressImporter(ILogger<ImporterBase<Item>> logger, IEntryMapper<Item> entryMapper)
    : ImporterBase<Item>(logger, entryMapper)
{
    protected override string SourceSystemName => "WordPress";
    protected override string MediaFolderRoot => "/Users/ian/dev/WordPress/media/";
    protected override IList<Item> LoadSourceItems()
    {
        using var openStream = File.OpenRead("/Users/ian/dev/WordPress/wordpress.xml");
        
        var ser = new XmlSerializer(typeof(Rss));

        var rss = ser.Deserialize(openStream) as Rss;

        return rss == null
            ? throw new InvalidOperationException("Could not deserialize WordPress file to Rss")
            : rss.Channel.Items;
    }

    protected override IList<Item> FilterSourceItems(IList<Item> sourceItems)
    {
        return sourceItems;
    }
}