using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Twitter.Model;

public class Url
{
    [JsonPropertyName("url")]
    public required string IncludedUrl { get; init; }
    
    [JsonPropertyName("expanded_url")]
    public required string ExpandedUrl { get; init; }
}