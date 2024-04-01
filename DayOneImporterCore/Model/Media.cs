using System.Text.Json.Serialization;

namespace DayOneImporterCore.Model;

public class Media
{
    [JsonPropertyName("md5")]
    public string Md5 { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "jpeg";

    [JsonIgnore]
    public string SourceLocation { get; set; }
}