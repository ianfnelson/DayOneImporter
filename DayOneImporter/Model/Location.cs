using System.Text.Json.Serialization;

namespace DayOneImporter.Model;

public class Location
{
    [JsonPropertyName("placeName")]
    public string PlaceName { get; set; }
    
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
    
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
}