namespace DayOneImporter;

public interface IImporter
{
    void Import(DateTimeOffset startDate);
}