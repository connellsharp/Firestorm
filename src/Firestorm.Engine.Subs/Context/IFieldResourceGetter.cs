using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Fields;

namespace Firestorm.Stems.Fuel.Fields
{
    /// <summary>
    /// Handler just for stems that can get the full resource.
    /// </summary>
    public interface IFieldResourceGetter<in TItem> : IFieldHandler<TItem>
    {
        IRestResource GetFullResource(IDeferredItem<TItem> item, IDataTransaction dataTransaction);
    }
}