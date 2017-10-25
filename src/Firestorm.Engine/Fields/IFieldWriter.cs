using System.Threading.Tasks;

namespace Firestorm.Engine.Fields
{
    /// <summary>
    /// Sets field values from an API request.
    /// </summary>
    public interface IFieldWriter<in TItem> : IFieldHandler<TItem>
        where TItem : class
    {
        Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction);
    }
}