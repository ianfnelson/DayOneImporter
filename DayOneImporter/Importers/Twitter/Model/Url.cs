using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Twitter.Model;

public class Url
{
    [JsonPropertyName("url")]
    public string IncludedUrl { get; set; }
    
    [JsonPropertyName("expanded_url")]
    public string ExpandedUrl { get; set; }
}