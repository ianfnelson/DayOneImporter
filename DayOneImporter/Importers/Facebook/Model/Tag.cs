using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class Tag
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}