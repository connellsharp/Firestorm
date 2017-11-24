using System;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Writers
{
    /// <summary>
    /// Uses a separate getter expression and setter action to map the fields.
    /// </summary>
    public class ActionFieldWriter<TItem, TValue> : IFieldWriter<TItem>
        where TItem : class
    {
        private readonly Action<TItem, TValue> _setterAction;

        public ActionFieldWriter([NotNull] Action<TItem, TValue> setterAction)
        {
            if (setterAction == null) throw new ArgumentNullException(nameof(setterAction));
            _setterAction = setterAction;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            await item.LoadAsync(); // TODO: and do we want to load items within the set function?

            TValue newValue = ConversionUtility.CleverConvertValue<TValue>(deserializedValue);
            _setterAction(item.LoadedItem, newValue);
        }
    }
}