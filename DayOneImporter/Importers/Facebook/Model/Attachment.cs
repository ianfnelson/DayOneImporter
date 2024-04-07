using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class Attachment
{
    [JsonPropertyName("data")]
    public List<AttachmentDataItem> Data { get; set; }
}