using System.Text.Json.Serialization;

namespace DayOneImporterCore;

public class Metadata
{
    [JsonPropertyName("metadata")]
    public string Version { get; set; } = "1.0";
}

public class DayOneFile
{
    [JsonPropertyName("metadata")] public Metadata Metadata { get; set; } = new();
    
    [JsonPropertyName("entries")]
    public IList<Entry> Entries { get; set; }
}

public class Entry
{
    [JsonPropertyName("creationDate")]
    public DateTimeOffset CreationDate { get; set; }

    [JsonPropertyName("timeZone")] 
    public string TimeZone { get; set; } = @"Europe/London";
    
    // TODO - photos
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}