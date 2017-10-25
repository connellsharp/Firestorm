using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

// ReSharper disable ConsiderUsingAsyncSuffix

namespace Firestorm.Client
{
    public static class RestItemDataExtensions
    {
        const BindingFlags PROPERTY_BINDING_FLAGS = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

        public static T As<T>(this RestItemData itemData)
            where T : class, new()
        {
            // TODO: possibly deffered RestItemData Stream so we can use newtonsoft directly?

            var item = new T();
            Type type = typeof(T);

            foreach (KeyValuePair<string, object> pair in itemData)
            {
                PropertyInfo property = type.GetProperty(pair.Key, PROPERTY_BINDING_FLAGS);
                object convertedValue = Convert.ChangeType(pair.Value, property.PropertyType);
                property.SetValue(item, convertedValue);
            }

            return item;
        }

        public static async Task<T> As<T>(this Task<RestItemData> itemDataTask)
            where T : class, new()
        {
            RestItemData itemData = await itemDataTask;
            return itemData.As<T>();
        }

        public static IEnumerable<T> As<T>(this IEnumerable<RestItemData> itemDataList)
            where T : class, new()
        {
            foreach (RestItemData itemData in itemDataList)
            {
                yield return itemData.As<T>();
            }
        }

        public static async Task<IEnumerable<T>> As<T>(this Task<IEnumerable<RestItemData>> itemDataListTask)
            where T : class, new()
        {
            IEnumerable<RestItemData> itemDataList = await itemDataListTask;
            return itemDataList.As<T>();
        }
    }
}