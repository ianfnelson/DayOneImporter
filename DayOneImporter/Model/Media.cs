using System.Text.Json.Serialization;

namespace DayOneImporter.Model;

public class Media
{
    [JsonPropertyName("md5")]
    public required string Md5 { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "jpeg";

    [JsonIgnore]
    public string SourceLocation { get; set; }
}