using System.Text.Json.Serialization;

namespace DayOneImporterCore.Facebook;

public class Post : ISourceItem
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("data")]
    public List<PostDataItem> Data { get; set; }
    
    [JsonPropertyName("tags")]
    public List<Tag> Tags { get; set; }
    
    [JsonPropertyName("attachments")]
    public List<Attachment> Attachments { get; set; }
}

public class Attachment
{
    [JsonPropertyName("data")]
    public List<AttachmentDataItem> Data { get; set; }
}

public class AttachmentDataItem
{
    [JsonPropertyName("media")]
    public Media Media { get; set; }
    
    [JsonPropertyName("external_context")]
    public ExternalContext ExternalContext { get; set; }
    
    [JsonPropertyName("place")]
    public Place Place { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class Place
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("coordinate")]
    public Coordinate Coordinate { get; set; }
}

public class Coordinate
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
    
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}

public class ExternalContext
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public class Media
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; }
    
    [JsonPropertyName("creation_timestamp")]
    public long? CreationTimestamp { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Tag
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class PostDataItem
{
    [JsonPropertyName("post")]
    public string Post { get; set; }
    
    [JsonPropertyName("update_timestamp")]
    public long? UpdateTimestamp { get; set; }
}