using System.Threading.Tasks;

namespace Firestorm.Engine.Deferring
{
    /// <summary>
    /// A deferred item for the Engine, which has in fact already been loaded into memory.
    /// </summary>
    internal class AlreadyLoadedItem<TItem> : IDeferredItem<TItem>
    {
        public AlreadyLoadedItem(TItem alreadyLoadedItem, string identifierUsed)
        {
            Identifier = identifierUsed;
            Query = new[] { alreadyLoadedItem }.SingleDefferred();
            LoadedItem = alreadyLoadedItem;
        }

        public string Identifier { get; }

        public IQueryableSingle<TItem> Query { get; }

        public TItem LoadedItem { get; }

        public Task LoadAsync()
        {
            return Task.FromResult(false);
        }

        public bool WasCreated { get; } = false;
    }
}