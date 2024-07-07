using System.Text;
using DayOneImporter.Importers.Albums.Model;
using DayOneImporter.Model;
using Microsoft.Extensions.Primitives;

namespace DayOneImporter.Importers.Albums;

public class AlbumMapper : IEntryMapper<Album>
{
    public Entry Map(Album sourceItem, string mediaFolderRoot)
    {
        var entryDate = BuildEntryDate(sourceItem);

        return new Entry
        {
            CreationDate = entryDate,
            ModifiedDate = entryDate,
            IsAllDay = true,
            Text = BuildText(sourceItem)
        };
    }
    
    private static DateTimeOffset BuildEntryDate(Album sourceItem)
    {
        return new DateTimeOffset(sourceItem.HeardForProject);
    }

    private static string BuildText(Album sourceItem)
    {
        var sb = new StringBuilder();

        sb.Append($"{sourceItem.Title} by {sourceItem.Artist}").Append("\n\n");

        sb.Append($"Artist: {sourceItem.Artist}").Append("\n");
        sb.Append($"Album: {sourceItem.Title}").Append("\n");
        sb.Append($"Year: {sourceItem.Year}").Append("\n");
        sb.Append($"Heard Before: {sourceItem.HeardBefore}").Append("\n");
        sb.Append($"Originally Heard: {sourceItem.OriginallyHeard}").Append("\n");
        sb.Append($"Rating: {sourceItem.Rating}/10");

        return sb.ToString();

    }
}