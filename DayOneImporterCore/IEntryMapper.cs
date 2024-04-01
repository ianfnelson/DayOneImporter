using DayOneImporterCore.Model;

namespace DayOneImporterCore;

public interface IEntryMapper<in TSourceItem> where TSourceItem : ISourceItem
{
    Entry Map(TSourceItem sourceItem, string mediaFolderRoot);
}