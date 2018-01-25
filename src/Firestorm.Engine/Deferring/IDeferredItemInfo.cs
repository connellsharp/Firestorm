namespace Firestorm.Engine
{
    public interface IDeferredItemInfo
    {
        /// <summary>
        /// The string identifier used to describe the item.
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Returns true if the item was created upon calling <see cref="IDeferredItem{T}.LoadAsync"/>.
        /// </summary>
        bool WasCreated { get; }
    }
}