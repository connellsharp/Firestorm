using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Firestorm.Data;

namespace Firestorm.Engine
{
    /// <summary>
    /// An all-powerful super conversion tool to try convert anything from anything.
    /// </summary>
    public static class ConversionUtility
    {
        public static object ConvertValue(object value, Type type)
        {
            if (IsNull(value, type))
                return null;

            if (type.IsInstanceOfType(value))
                return value;

            type = Nullable.GetUnderlyingType(type) ?? type;

            string strValue = value.ToString();

            return ConvertString(strValue, type);
        }

        private static bool IsNull(object value, Type type)
        {
            if (value == null)
            {
                if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
                    throw new ArgumentException("Cannot convert the given null value to a value type.");

                return true;
            }

            return false;
        }

        public static T ConvertValue<T>(object value)
        {
            return (T) ConvertValue(value, typeof(T));
        }

        public static object ConvertString(string strValue, Type type)
        {
            // TODO if type IS string?

            if (string.IsNullOrEmpty(strValue))
                return null;

            if (type == typeof(DateTime) && strValue.Contains("Date("))
            {
                // TODO: implement MS date format
                throw new NotImplementedException("MS date format not implemented yet.");
            }

            int number;
            if (type.IsEnum && int.TryParse(strValue, out number))
                return Enum.ToObject(type, number);

            return Convert.ChangeType(strValue, type);
        }

        public static T ConvertString<T>(string strValue)
        {
            return (T) ConvertString(strValue, typeof(T));
        }

        public static TValue CleverConvertValue<TValue>(object sourceValue)
        {
            return (TValue) CleverConvertValue(sourceValue, typeof(TValue));
        }

        public static object CleverConvertValue(object sourceValue, Type type)
        {
            if (IsNull(sourceValue, type))
                return null;

            if (type.IsInstanceOfType(sourceValue))
                return sourceValue;

            if (EnumerableTypeUtility.IsEnumerable(type))
            {
                Type itemType = EnumerableTypeUtility.GetItemType(type);
                IList newList = EnumerableTypeUtility.CreateList(itemType);

                CopyArrayItems((IEnumerable) sourceValue, itemType, newList);

                if (type.IsArray)
                {
                    Array array = Array.CreateInstance(itemType, newList.Count);
                    newList.CopyTo(array, 0);
                    return array;
                }

                return newList;
            }
            else
            {
                var newValue = Activator.CreateInstance(type);
                CopyProperties(sourceValue, newValue);
                return newValue;
            }
        }

        private static void CopyArrayItems(IEnumerable sourceValue, Type itemType, IList newList)
        {
            if (itemType == typeof(string))
            {
                foreach (object sourceItem in sourceValue)
                {
                    newList.Add(sourceItem.ToString());
                }
            }
            else
            {
                foreach (object sourceItem in sourceValue)
                {
                    var newItem = Activator.CreateInstance(itemType);
                    CopyProperties(sourceItem, newItem);
                    newList.Add(newItem);
                }
            }
        }

        private static object CopyProperties(object sourceValue, object dest)
        {
            PropertyDescriptorCollection destProps = TypeDescriptor.GetProperties(dest);

            var dictionary = sourceValue as IDictionary<string, object>;
            if (dictionary != null)
            {
                foreach (var pair in dictionary)
                {
                    PropertyDescriptor destProp = destProps[pair.Key];
                    destProp?.SetValue(dest, pair.Value);
                }
            }
            else
            {
                PropertyDescriptorCollection sourceProps = TypeDescriptor.GetProperties(sourceValue);

                foreach (PropertyDescriptor prop in sourceProps)
                {
                    PropertyDescriptor destProp = destProps[prop.Name];
                    var sourcePropValue = prop.GetValue(sourceValue);
                    var convertedValue = CleverConvertValue(sourcePropValue, destProp.PropertyType);
                    destProp?.SetValue(dest, convertedValue);
                }
            }

            return dest;
        }
    }
}