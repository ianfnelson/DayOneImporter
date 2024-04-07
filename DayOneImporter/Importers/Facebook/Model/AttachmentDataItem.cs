using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class AttachmentDataItem
{
    [JsonPropertyName("media")]
    public FbMedia? FbMedia { get; init; }
    
    [JsonPropertyName("external_context")]
    public ExternalContext? ExternalContext { get; init; }
    
    [JsonPropertyName("place")]
    public Place? Place { get; init; }
    
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}