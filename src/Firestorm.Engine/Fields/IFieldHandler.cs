namespace Firestorm.Engine.Fields
{
    /// <summary>
    /// Base interface for <see cref="IFieldReader{TItem}"/> and <see cref="IFieldWriter{TItem}"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the parent item that this field belongs to.</typeparam>
    public interface IFieldHandler<in TItem>
    { }
}