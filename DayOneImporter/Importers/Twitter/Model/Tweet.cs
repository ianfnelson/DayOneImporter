using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Twitter.Model;

public class Tweet : ISourceItem
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }
    
    [JsonPropertyName("full_text")]
    public required string FullText { get; init; }
    
    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; init; }
    
    [JsonPropertyName("entities")]
    public Entities? Entities { get; init; }
    
    [JsonPropertyName("extended_entities")]
    public ExtendedEntities? ExtendedEntities { get; init; }
}