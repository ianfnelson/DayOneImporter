using System.Text.Json.Serialization;

namespace DayOneImporterCore.Model;

public class Entry
{
    [JsonPropertyName("creationDate")]
    public DateTimeOffset CreationDate { get; set; }
    
    [JsonPropertyName("modifiedDate")]
    public DateTimeOffset ModifiedDate { get; set; }

    [JsonPropertyName("timeZone")] 
    public string TimeZone { get; set; } = "Europe/London";
    
    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("photos")] 
    public List<Media> Photos { get; set; } = new();

    [JsonPropertyName("videos")] 
    public List<Media> Videos { get; set; } = new();

    [JsonPropertyName("tags")] 
    public List<string> Tags { get; set; } = new();
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}