using System.IO.Compression;
using System.Text.Json;
using DayOneImporter.Model;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace DayOneImporter;

public abstract class ImporterBase<TSourceItem>(
    ILogger<ImporterBase<TSourceItem>> logger,
    IEntryMapper<TSourceItem> entryMapper)
    : IImporter
    where TSourceItem : ISourceItem
{
    protected readonly ILogger<ImporterBase<TSourceItem>> Logger = logger;

    protected virtual int BatchSize => 200;

    protected abstract string SourceSystemName { get; }

    protected abstract string MediaFolderRoot { get; }
    
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
            var entry = entryMapper.Map(sourceItem, MediaFolderRoot);

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
    
    private static void WriteOutputFile(IList<Entry> mappedEntries, string batchFolderName)
    {
        var dayOneFile = new DayOneFile { Entries = mappedEntries };

        using var createStream = File.Create( batchFolderName +"/journal.json");

        var options = new JsonSerializerOptions { WriteIndented = true };
        options.Converters.Add(new CustomDateTimeOffsetConverter("yyyy-MM-ddTHH:mm:ssZ"));
        
        JsonSerializer.Serialize(createStream, dayOneFile, options);
    }
}