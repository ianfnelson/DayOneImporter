using DayOneImporter.Importers.Albums.Model;
using TinyCsvParser.Mapping;

namespace DayOneImporter.Importers.Albums;

public class CsvAlbumMapping : CsvMapping<Album>
{
    public CsvAlbumMapping()
    {
        MapProperty(1, x => x.Artist);
        MapProperty(2, x => x.Title);
        MapProperty(3, x => x.Year);
        MapProperty(5, x => x.HeardBefore);
        MapProperty(6, x => x.OriginallyHeard);
        MapProperty(7, x => x.HeardForProject);
        MapProperty(8, x => x.Rating);
    }
}