using System;
using System.Reflection;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Writers
{
    /// <summary>
    /// Uses <see cref="PropertyInfo"/> of a property to write to a field using reflection.
    /// </summary>
    public class PropertyInfoFieldWriter<TItem> : IFieldWriter<TItem>
        where TItem : class
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyInfoFieldWriter([NotNull] PropertyInfo propertyInfo)
        {
            PropertyInfoUtilities.EnsureValidProperty<TItem>(propertyInfo);
            _propertyInfo = propertyInfo;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            await item.LoadAsync();

            if (item.LoadedItem == null)
                throw new ArgumentNullException(nameof(item.LoadedItem), "Cannot set field value for this item because the item is null.");

            object convertedValue = ConversionUtility.ConvertValue(deserializedValue, _propertyInfo.PropertyType);
            _propertyInfo.SetValue(item.LoadedItem, convertedValue);
        }
    }
}