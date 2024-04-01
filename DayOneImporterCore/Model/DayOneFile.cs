using System.Text.Json.Serialization;

namespace DayOneImporterCore.Model;

public class DayOneFile
{
    [JsonPropertyName("metadata")] public Metadata Metadata { get; set; } = new();
    
    [JsonPropertyName("entries")]
    public IList<Entry> Entries { get; set; }
}