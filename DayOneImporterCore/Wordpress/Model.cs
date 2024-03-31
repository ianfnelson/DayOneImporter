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
    
    [XmlElement(ElementName="post_date_gmt", Namespace = "http://wordpress.org/export/1.2/")]
    public string PostDateGmt { get; set; }
    
    [XmlElement(ElementName = "post_modified_gmt", Namespace = "http://wordpress.org/export/1.2/")]
    public string PostModifiedGmt { get; set; }
    
    [XmlElement(ElementName = "encoded", Namespace = "http://purl.org/rss/1.0/modules/content/")]
    public string Content { get; set; }
}