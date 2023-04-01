using System.Text.Json.Serialization;

namespace DayOneImporterCore.Facebook;

public class Post : ISourceItem
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("data")]
    public List<PostDataItem> Data { get; set; }
}

public class PostDataItem
{
    [JsonPropertyName("post")]
    public string Post { get; set; }
    
    [JsonPropertyName("update_timestamp")]
    public long? UpdateTimestamp { get; set; }
}