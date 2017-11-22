using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Fields
{
    /// <summary>
    /// A mapping of given API fields used for Stems.
    /// </summary>
    public interface ILocatableFieldProvider<TItem> : IFieldProvider<TItem>
        where TItem : class
    {
        [CanBeNull]
        IItemLocator<TItem> GetLocator(string fieldName);
    }
}