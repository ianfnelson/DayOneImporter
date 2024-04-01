using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Twitter.Model;

public class Entities
{
    [JsonPropertyName("urls")]
    public List<Url> Urls { get; set; }
}