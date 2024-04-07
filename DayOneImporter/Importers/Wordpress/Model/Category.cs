using System.Xml.Serialization;

namespace DayOneImporter.Importers.Wordpress.Model;

public class Category
{
    [XmlAttribute("domain")]
    public required string Domain { get; init; }
    
    [XmlText]
    public required string Value { get; init; }
}