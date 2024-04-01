using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Twitter.Model;

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
    
    [JsonPropertyName("extended_entities")]
    public ExtendedEntities ExtendedEntities { get; set; }
}