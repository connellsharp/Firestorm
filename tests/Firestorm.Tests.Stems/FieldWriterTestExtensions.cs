using System.Threading.Tasks;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Fuel.Substems.Repositories;

namespace Firestorm.Tests.Stems
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