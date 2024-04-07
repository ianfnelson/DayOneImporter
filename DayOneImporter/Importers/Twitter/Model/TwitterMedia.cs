using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Twitter.Model;

public class TwitterMedia
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("media_url")]
    public string MediaUrl { get; set; }

    [JsonPropertyName("video_info")]
    public object VideoInfo { get; set; }
}