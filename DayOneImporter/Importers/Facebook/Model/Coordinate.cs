using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class Coordinate
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; init; }
    
    [JsonPropertyName("longitude")]
    public double Longitude { get; init; }
}