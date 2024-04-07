using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Twitter.Model;

public class ExtendedEntities
{
    [JsonPropertyName("media")]
    public List<TwitterMedia>? Media { get; init; }
}