using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Deferring;

namespace Firestorm.Engine.Fields
{
    /// <summary>
    /// Sets field values from an API request.
    /// </summary>
    public interface IFieldWriter<in TItem> : IFieldHandler<TItem>
        where TItem : class
    {
        Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction); // TODO remove dataTransaction
    }
}