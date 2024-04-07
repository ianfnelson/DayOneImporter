using System.Text.Json.Serialization;

namespace DayOneImporter.Model;

public class DayOneFile
{
    [JsonPropertyName("metadata")] public Metadata Metadata { get; set; } = new();
    
    [JsonPropertyName("entries")]
    public IList<Entry> Entries { get; set; }
}