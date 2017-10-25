namespace Firestorm.Engine
{
    /// <summary>
    /// The lazy-loaded item state of a <see cref="DeferredItemBase{TItem}"/>.
    /// </summary>
    internal enum LazyState
    {
        /// <summary>
        /// The item has not been loaded yet.
        /// </summary>
        NotLoaded,

        /// <summary>
        /// Attempted to load the item but no item was found.
        /// </summary>
        NoItemFound,

        /// <summary>
        /// No item was found, but a new item has been created.
        /// </summary>
        Created,

        /// <summary>
        /// An item was returned, but that item was null.
        /// </summary>
        NullValue,

        /// <summary>
        /// An item has successfully loaded.
        /// </summary>
        LoadedItem
    }
}