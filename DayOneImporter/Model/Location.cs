using System.Text.Json.Serialization;

namespace DayOneImporter.Model;

public class Location
{
    [JsonPropertyName("placeName")]
    public required string PlaceName { get; init; }
    
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
    
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
}