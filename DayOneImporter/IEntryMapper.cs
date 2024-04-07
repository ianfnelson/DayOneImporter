using DayOneImporter.Model;

namespace DayOneImporter;

public interface IEntryMapper<in TSourceItem> where TSourceItem : ISourceItem
{
    Entry Map(TSourceItem sourceItem, string mediaFolderRoot);
}