using System.Xml.Serialization;

namespace DayOneImporterCore.Wordpress;

[XmlRoot("rss")]
public class Rss
{
    [XmlElement("channel")]
    public Channel Channel { get; set; }
}

public class Channel
{
    [XmlElement("item")]
    public List<Item> Items { get; set; }
}

public class Item : ISourceItem
{
    [XmlElement("title")]
    public string Title { get; set; }
}