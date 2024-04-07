using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class Place
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("coordinate")]
    public Coordinate Coordinate { get; set; }
}