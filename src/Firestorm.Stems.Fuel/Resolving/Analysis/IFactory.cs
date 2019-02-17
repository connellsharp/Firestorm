using Firestorm.Engine.Fields;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    /// <summary>
    /// Provides a method to get an <see cref="Firestorm.Engine.Fields.IFieldHandler{TItem}"/> object from a <see cref="Stem{TItem}"/>.
    /// Used for optimization to allow reflection to build the handler factories before the stem is known and keep a reusable cache of type data.
    /// </summary>
    public interface IFactory<out TFieldHandler, TItem>
        where TFieldHandler : IFieldHandler<TItem>
        where TItem : class
    {
        TFieldHandler Get(Stem<TItem> stem);
    }
}