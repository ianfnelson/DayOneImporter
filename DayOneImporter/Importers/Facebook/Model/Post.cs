using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class Post : ISourceItem
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("data")]
    public List<PostDataItem> Data { get; set; }
    
    [JsonPropertyName("tags")]
    public List<Tag> Tags { get; set; }
    
    [JsonPropertyName("attachments")]
    public List<Attachment> Attachments { get; set; }
}