using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class ExternalContext
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}