using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Facebook.Model;

public class PostDataItem
{
    [JsonPropertyName("post")]
    public string Post { get; set; }
    
    [JsonPropertyName("update_timestamp")]
    public long? UpdateTimestamp { get; set; }
}