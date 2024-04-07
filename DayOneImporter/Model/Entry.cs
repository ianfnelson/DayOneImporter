using System.Text.Json.Serialization;

namespace DayOneImporter.Model;

public class Entry
{
    [JsonPropertyName("creationDate")]
    public DateTimeOffset CreationDate { get; init; }
    
    [JsonPropertyName("modifiedDate")]
    public DateTimeOffset ModifiedDate { get; init; }

    [JsonPropertyName("timeZone")] 
    public static string TimeZone => "Europe/London";

    [JsonPropertyName("location")]
    public Location? Location { get; init; }

    [JsonPropertyName("photos")] 
    public List<Media> Photos { get; init; } = [];

    [JsonPropertyName("videos")] 
    public List<Media> Videos { get; init; } = [];

    [JsonPropertyName("tags")] 
    public List<string> Tags { get; init; } = [];
    
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}