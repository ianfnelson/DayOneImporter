using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class Attachment
{
    [JsonPropertyName("data")]
    public required List<AttachmentDataItem> Data { get; init; }
}