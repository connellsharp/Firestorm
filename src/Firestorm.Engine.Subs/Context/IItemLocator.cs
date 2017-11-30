using Firestorm.Data;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Subs.Context
{
    /// <summary>
    /// Locates an item by a field's value when used within a parent.
    /// </summary>
    public interface IItemLocator<TItem> : IFieldHandler<TItem>
        where TItem : class
    {
        TItem TryLocateItem(IEngineRepository<TItem> navRepository, object findValue);
    }
}