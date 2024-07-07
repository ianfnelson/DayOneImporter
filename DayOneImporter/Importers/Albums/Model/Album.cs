namespace DayOneImporter.Importers.Albums.Model;

public class Album : ISourceItem
{
    public string Artist { get; set; }
    
    public string Title { get; set; }
    
    public string Year { get; set; }
    
    public string HeardBefore { get; set; }
    
    public string OriginallyHeard { get; set; }
    
    public DateTime HeardForProject { get; set; }
    
    public string Rating { get; set; }
}