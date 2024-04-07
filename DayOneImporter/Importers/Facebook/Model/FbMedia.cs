using System.Text.Json.Serialization;

namespace DayOneImporter.Importers.Facebook.Model;

public class FbMedia
{
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
}