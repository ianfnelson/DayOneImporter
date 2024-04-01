using System.Text.Json.Serialization;

namespace DayOneImporterCore.Importers.Twitter.Model;

public class Record 
{
    [JsonPropertyName("tweet")]
    public Tweet Tweet { get; set; }
}