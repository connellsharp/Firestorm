using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Stems.Tests
{
    public static class FieldWriterTestExtensions
    {
        public static Task SetValueAsync<TItem>(this IFieldWriter<TItem> writer, TItem item, object deserializedValue, IDataTransaction dataTransaction)
            where TItem : class
        {
            var loadedItem = new LoadedItem<TItem>(item);
            return writer.SetValueAsync(loadedItem, deserializedValue, dataTransaction);
        }
    }
}