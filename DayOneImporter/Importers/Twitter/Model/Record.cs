using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Twitter.Model;

public class Record 
{
    [JsonPropertyName("tweet")]
    public Tweet Tweet { get; set; }
}