using System.Text.Json.Serialization;

namespace DayOneImporter.Model;

public class Metadata
{
    [JsonPropertyName("metadata")]
    public string Version { get; set; } = "1.0";
}