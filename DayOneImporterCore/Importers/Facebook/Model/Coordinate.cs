using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Facebook.Model;

public class Coordinate
{
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
    
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
}