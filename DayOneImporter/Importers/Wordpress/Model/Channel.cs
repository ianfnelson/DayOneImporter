using System.Xml.Serialization;

namespace DayOneImporter.Importers.Wordpress.Model;

public class Channel
{
    [XmlElement("item")]
    public required List<Item> Items { get; set; }
}