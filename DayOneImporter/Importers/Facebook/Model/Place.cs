using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class Place
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("coordinate")]
    public Coordinate? Coordinate { get; init; }
}