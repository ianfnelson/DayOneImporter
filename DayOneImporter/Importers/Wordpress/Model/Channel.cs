using System.Xml.Serialization;

namespace DayOneImporter.Importers.Wordpress.Model;

public class Channel
{
    [XmlElement("item")]
    public List<Item> Items { get; set; }
}