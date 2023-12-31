using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace DayOneImporterCore;

public interface IImporter
{
    void Import(DateTimeOffset startDate);
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
    
    public virtual int BatchSize { get; } = 200;
    
    public abstract string SourceSystemName { get; }
    
    public abstract string MediaFolderRoot { get; }
    
    public void Import(DateTimeOffset startDate)
    {
        Logger.LogInformation("Beginning " + SourceSystemName + " import");
        var sourceItems = LoadSourceItems();
        Logger.LogInformation("Loaded " + sourceItems.Count + " source items");
        
        sourceItems = FilterSourceItems(sourceItems);
        Logger.LogInformation("Following filtering, " + sourceItems.Count + " source items remain");

        int batchNumber = 1;
        foreach (var batch in MapEntries(sourceItems, startDate).Batch(BatchSize))
        {
            var batchList = batch.ToList();
            BuildOutputZip(batchList, batchNumber);
            Logger.LogInformation("Done batch " + batchNumber);
            batchNumber++;
        }
    }

    protected abstract IList<TSourceItem> LoadSourceItems();

    protected abstract IList<TSourceItem> FilterSourceItems(IList<TSourceItem> sourceItems);

    protected virtual IEnumerable<Entry> MapEntries(IEnumerable<TSourceItem> sourceItems, DateTimeOffset startDate)
    {
        foreach (var sourceItem in sourceItems)
        {
            var entry = _entryMapper.Map(sourceItem, MediaFolderRoot);

            if (string.IsNullOrEmpty(entry.Text) &&
                (entry.Photos == null || entry.Photos.Count == 0))
            {
                continue;
            }

            if (entry.CreationDate < startDate)
            {
                continue;
            }

            yield return entry;
        }
    }

    protected virtual void BuildOutputZip(IList<Entry> mappedEntries, int batchNumber)
    {
        var batchFolderName = "/Users/ian/dev/DayOneOutput/" + SourceSystemName + "/" + batchNumber;
        
        Directory.CreateDirectory(batchFolderName);
        WriteOutputFile(mappedEntries, batchFolderName);
        
        CopyOutputMedia(mappedEntries, batchFolderName);
        
        ZipFile.CreateFromDirectory(batchFolderName, "/Users/ian/dev/DayOneOutput/" + SourceSystemName + "/" + batchNumber + ".zip");
    }

    private void CopyOutputMedia(IList<Entry> mappedEntries, string batchFolderName)
    {
        var photoFolderName = batchFolderName + "/photos";
        
        Directory.CreateDirectory(photoFolderName);

        foreach (var photo in mappedEntries.SelectMany(x => x.Photos))
        {
            File.Copy(MediaFolderRoot + photo.SourceLocation, photoFolderName + "/" + photo.Md5 + photo.SourceLocation.Substring(photo.SourceLocation.LastIndexOf(".")), true);
        }
        
        var videoFolderName = batchFolderName + "/videos";
        
        Directory.CreateDirectory(videoFolderName);

        foreach (var video in mappedEntries.SelectMany(x => x.Videos))
        {
            File.Copy(MediaFolderRoot + video.SourceLocation, videoFolderName + "/" + video.Md5 + video.SourceLocation.Substring(video.SourceLocation.LastIndexOf(".")), true);
        }
    }
    
    private void WriteOutputFile(IList<Entry> mappedEntries, string batchFolderName)
    {
        var dayOneFile = new DayOneFile { Entries = mappedEntries };

        using FileStream createStream = File.Create( batchFolderName +"/journal.json");

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
    Entry Map(TSourceItem sourceItem, string mediaFolderRoot);
}