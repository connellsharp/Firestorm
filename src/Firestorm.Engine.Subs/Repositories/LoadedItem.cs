using Firestorm.Engine.Defaults;

namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// An item that has already been loaded into memory.
    /// </summary>
    internal class LoadedItem<TItem> : RepositoryDeferredItem<TItem>
        where TItem : class
    {
        public LoadedItem(TItem loadedItem)
            : base(new LoadedItemRepository<TItem>(loadedItem), new MemoryAsyncQueryer())
        {
        }
    }
}