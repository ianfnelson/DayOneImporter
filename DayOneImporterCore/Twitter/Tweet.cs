using System.Text.Json.Serialization;

namespace DayOneImporterCore.Twitter;

public class Record 
{
    [JsonPropertyName("tweet")]
    public Tweet Tweet { get; set; }
}

public class Tweet : ISourceItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("full_text")]
    public string FullText { get; set; }
    
    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }
    
    [JsonPropertyName("in_reply_to_user_id")]
    public string ReplyUserId { get; set; }
    
    [JsonPropertyName("in_reply_to_status_id")]
    public string ReplyStatusId { get; set; }
    
    [JsonPropertyName("entities")]
    public Entities Entities { get; set; }
}

public class Entities
{
    [JsonPropertyName("urls")]
    public List<Url> Urls { get; set; }
}

public class Url
{
    [JsonPropertyName("url")]
    public string IncludedUrl { get; set; }
    
    [JsonPropertyName("expanded_url")]
    public string ExpandedUrl { get; set; }
}