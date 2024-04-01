using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Twitter.Model;

public class ExtendedEntities
{
    [JsonPropertyName("media")]
    public List<TwitterMedia> Media { get; set; }
}