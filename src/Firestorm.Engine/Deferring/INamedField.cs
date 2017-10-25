using Firestorm.Engine.Fields;

namespace Firestorm.Engine
{
    /// <summary>
    /// A reference to an item's field, as described by an API user.
    /// </summary>
    /// <remarks>
    /// Similar concept to <see cref="IDeferredItem{TItem}"/>.
    /// </remarks>
    public interface INamedField<in TItem>
        where TItem : class
    {
        /// <summary>
        /// The name used to describe the field.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The field writer from the engine.
        /// </summary>
        IFieldReader<TItem> Reader { get; }

        /// <summary>
        /// The field reader from the engine.
        /// </summary>
        IFieldWriter<TItem> Writer { get; }
    }
}