using System.Text.Json.Serialization;

namespace DayOneImporterCore.Model;

public class Metadata
{
    [JsonPropertyName("metadata")]
    public string Version { get; set; } = "1.0";
}