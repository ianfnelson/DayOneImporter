using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class Post : ISourceItem
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; init; }
    
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    
    [JsonPropertyName("data")]
    public List<PostDataItem>? Data { get; init; }
    
    [JsonPropertyName("tags")]
    public List<Tag>? Tags { get; init; }
    
    [JsonPropertyName("attachments")]
    public List<Attachment>? Attachments { get; init; }
}