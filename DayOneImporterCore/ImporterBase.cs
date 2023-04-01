using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace DayOneImporterCore;

public interface IImporter
{
    void Import();
}

public abstract class ImporterBase<TSourceItem> : IImporter where TSourceItem : ISourceItem
{
    protected readonly ILogger<ImporterBase<TSourceItem>> Logger;
    
    private readonly IEntryMapper<TSourceItem> _entryMapper;

    public ImporterBase(ILogger<ImporterBase<TSourceItem>> logger, IEntryMapper<TSourceItem> entryMapper)
    {
        Logger = logger;
        _entryMapper = entryMapper;
    }
    
    public virtual int BatchSize { get; } = 100;
    
    public abstract string SourceSystemName { get; }
    
    public void Import()
    {
        Logger.LogInformation("Beginning " + SourceSystemName + " import");
        var sourceItems = LoadSourceItems();
        Logger.LogInformation("Loaded " + sourceItems.Count + " source items");
        
        sourceItems = FilterSourceItems(sourceItems);
        Logger.LogInformation("Following filtering, " + sourceItems.Count + " source items remain");

        int batchNumber = 1;
        foreach (var batch in MapEntries(sourceItems).Batch(BatchSize))
        {
            var batchList = batch.ToList();
            WriteOutputFile(batchList, batchNumber);
            Logger.LogInformation("Done batch " + batchNumber);
            batchNumber++;
        }
        
        var mappedEntries = MapEntries(sourceItems);
        Logger.LogInformation("Entries mapped");

    }

    protected abstract IList<TSourceItem> LoadSourceItems();

    protected abstract IList<TSourceItem> FilterSourceItems(IList<TSourceItem> sourceItems);

    protected virtual IEnumerable<Entry> MapEntries(IEnumerable<TSourceItem> sourceItems)
    {
        foreach (var sourceItem in sourceItems)
        {
            yield return _entryMapper.Map(sourceItem);
        }
    }

    protected virtual void WriteOutputFile(IList<Entry> mappedEntries, int batchNumber)
    {
        Directory.CreateDirectory("/Users/ian/dev/DayOneOutput/" + SourceSystemName + "/" + batchNumber);
        
        var dayOneFile = new DayOneFile { Entries = mappedEntries };

        using FileStream createStream = File.Create( "/Users/ian/dev/DayOneOutput/" + SourceSystemName + "/" + batchNumber+ "/journal.json");

        var options = new JsonSerializerOptions { WriteIndented = true };
        options.Converters.Add(new CustomDateTimeOffsetConverter("yyyy-MM-ddTHH:mm:ssZ"));
        
        JsonSerializer.Serialize(createStream, dayOneFile, options);
    }
}

public class CustomDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    private readonly string _format;

    public CustomDateTimeOffsetConverter(string format)
    {
        _format = format;
    }

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(), _format, null);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString(_format));
    }
}

public interface ISourceItem
{
    
}

public interface IEntryMapper<TSourceItem> where TSourceItem : ISourceItem
{
    Entry Map(TSourceItem sourceItem);
}