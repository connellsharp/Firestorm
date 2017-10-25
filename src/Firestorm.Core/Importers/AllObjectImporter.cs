using System;
using System.Collections.Generic;
using System.Reflection;

namespace Firestorm
{
    public class AllObjectImporter : IRestItemDataImporter
    {
        public bool CanImport(object obj)
        {
            return true;
        }

        public Type GetType(object obj)
        {
            return obj.GetType();
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues(object obj)
        {
            Type type = obj.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object value = propertyInfo.GetValue(obj);
                yield return new KeyValuePair<string, object>(propertyInfo.Name, value);
            }

            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                object value = fieldInfo.GetValue(obj);
                yield return new KeyValuePair<string, object>(fieldInfo.Name, value);
            }
        }
    }
}