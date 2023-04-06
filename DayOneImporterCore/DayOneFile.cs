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
    
    [JsonPropertyName("modifiedDate")]
    public DateTimeOffset ModifiedDate { get; set; }

    [JsonPropertyName("timeZone")] 
    public string TimeZone { get; set; } = @"Europe/London";
    
    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("photos")] 
    public List<Photo> Photos { get; set; } = new List<Photo>();

    [JsonPropertyName("tags")] 
    public List<string> Tags { get; set; } = new List<string>();
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class Photo
{
    [JsonPropertyName("md5")]
    public string Md5 { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "jpeg";

    [JsonIgnore]
    public string SourceLocation { get; set; }
}

public class Location
{
    [JsonPropertyName("placeName")]
    public string PlaceName { get; set; }
    
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
    
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
}