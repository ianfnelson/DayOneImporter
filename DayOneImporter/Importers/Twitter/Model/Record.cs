using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Twitter.Model;

public class Record 
{
    [JsonPropertyName("tweet")]
    public required Tweet Tweet { get; init; }
}