using System.Xml.Serialization;

namespace DayOneImporter.Importers.Wordpress.Model;

[XmlRoot("rss")]
public class Rss
{
    [XmlElement("channel")]
    public required Channel Channel { get; init; }
}