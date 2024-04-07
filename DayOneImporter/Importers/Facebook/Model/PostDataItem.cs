using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class PostDataItem
{
    [JsonPropertyName("post")]
    public string? Post { get; init; }
    
    [JsonPropertyName("update_timestamp")]
    public long? UpdateTimestamp { get; init; }
}