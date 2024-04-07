using System.Xml.Serialization;

namespace DayOneImporter.Importers.Wordpress.Model;

public class Item : ISourceItem
{
    [XmlElement("title")]
    public required string Title { get; init; }
    
    [XmlElement(ElementName="post_date_gmt", Namespace = "http://wordpress.org/export/1.2/")]
    public required string PostDateGmt { get; init; }
    
    [XmlElement(ElementName = "post_modified_gmt", Namespace = "http://wordpress.org/export/1.2/")]
    public required string PostModifiedGmt { get; init; }
    
    [XmlElement(ElementName = "encoded", Namespace = "http://purl.org/rss/1.0/modules/content/")]
    public required string Content { get; init; }
    
    [XmlElement("category")]
    public List<Category>? Categories { get; init; }
}