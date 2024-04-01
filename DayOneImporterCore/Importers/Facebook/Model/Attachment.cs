using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Facebook.Model;

public class Attachment
{
    [JsonPropertyName("data")]
    public List<AttachmentDataItem> Data { get; set; }
}