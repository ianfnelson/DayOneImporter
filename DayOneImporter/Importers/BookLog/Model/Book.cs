namespace DayOneImporter.Importers.BookLog.Model;

public class Book : ISourceItem
{
    public DateTime Date { get; set; }
    
    public string Title { get; set; }
    
    public string Kind { get; set; }
}