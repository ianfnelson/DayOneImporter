using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Facebook.Model;

public class ExternalContext
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}