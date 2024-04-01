using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Facebook.Model;

public class AttachmentDataItem
{
    [JsonPropertyName("media")]
    public FbMedia FbMedia { get; set; }
    
    [JsonPropertyName("external_context")]
    public ExternalContext ExternalContext { get; set; }
    
    [JsonPropertyName("place")]
    public Place Place { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}