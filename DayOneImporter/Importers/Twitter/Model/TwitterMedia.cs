using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Twitter.Model;

public class TwitterMedia
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }
    
    [JsonPropertyName("media_url")]
    public required string MediaUrl { get; init; }

    [JsonPropertyName("video_info")]
    public object? VideoInfo { get; init; }
}