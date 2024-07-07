using System.Text.Json.Serialization;

namespace DayOneImporter.Model;

public class Media
{
    [JsonPropertyName("md5")]
    public string Md5 { get; init; }

    [JsonPropertyName("type")]
    public string Type { get; init; } = "jpeg";

    [JsonIgnore]
    public string SourceLocation { get; init; }
}