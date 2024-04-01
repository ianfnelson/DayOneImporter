using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Facebook.Model;

public class FbMedia
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; }
    
    [JsonPropertyName("creation_timestamp")]
    public long? CreationTimestamp { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
}