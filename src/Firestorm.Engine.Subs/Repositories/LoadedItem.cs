namespace Firestorm.Stems.Fuel.Substems.Repositories
{
    /// <summary>
    /// An item that has already been loaded into memory.
    /// </summary>
    internal class LoadedItem<TItem> : RepositoryDeferredItem<TItem>
        where TItem : class
    {

        public LoadedItem(TItem loadedItem) 
            : base(new LoadedItemRepository<TItem>(loadedItem))
        {
        }
    }
}