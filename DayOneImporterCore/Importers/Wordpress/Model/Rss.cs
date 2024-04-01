using System.Xml.Serialization;

namespace DayOneImporterCore.Importers.Wordpress.Model;

[XmlRoot("rss")]
public class Rss
{
    [XmlElement("channel")]
    public Channel Channel { get; set; }
}