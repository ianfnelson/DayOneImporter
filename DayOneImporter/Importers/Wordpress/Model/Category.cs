using System.Xml.Serialization;

namespace DayOneImporter.Importers.Wordpress.Model;

public class Category
{
    [XmlAttribute("domain")]
    public string Domain { get; set; }
    
    [XmlText]
    public string Value { get; set; }
}