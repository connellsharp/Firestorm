using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Engine
{
    /// <summary>
    /// A reference to a single item described by an API user. Contains string identifier and the engine's <see cref="IQueryable"/>.
    /// </summary>
    public interface IDeferredItem<out TItem> : IDeferredItemInfo
    {
        /// <summary>
        /// The query that can be used to load the item.
        /// </summary>
        IQueryableSingle<TItem> Query { get; }

        /// <summary>
        /// The pre-loaded item that should be populated after <see cref="LoadAsync"/> is called.
        /// </summary>
        TItem LoadedItem { get; }

        /// <summary>
        /// Executes the <see cref="Query"/> and loads the single item in the <see cref="LoadedItem"/> property.
        /// Does nothing if the item is already loaded.
        /// Throws an exception if the query returns no items.
        /// </summary>
        Task LoadAsync();
    }
}