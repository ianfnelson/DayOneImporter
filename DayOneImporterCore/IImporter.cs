namespace DayOneImporterCore;

public interface IImporter
{
    void Import(DateTimeOffset startDate);
}