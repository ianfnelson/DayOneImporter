using System.Xml.Serialization;

namespace DayOneImporterCore.Importers.Wordpress.Model;

public class Channel
{
    [XmlElement("item")]
    public List<Item> Items { get; set; }
}