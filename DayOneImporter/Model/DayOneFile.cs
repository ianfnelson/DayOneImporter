using System.Text.Json.Serialization;

namespace DayOneImporter.Model;

public class DayOneFile
{
    [JsonPropertyName("entries")]
    public required IList<Entry> Entries { get; set; }
}