using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Firestorm.Engine
{
    /// <summary>
    /// A reference to a single item described by an API user. Contains string identifier and the engine's <see cref="IQueryable"/>.
    /// </summary>
    public interface IDeferredItem<out TItem>
    {
        /// <summary>
        /// The string identifier used to describe the item.
        /// </summary>
        string Identifier { get; }

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

        /// <summary>
        /// Returns true if the item was created upon calling <see cref="LoadAsync"/>.
        /// </summary>
        bool WasCreated { get; }
    }
}