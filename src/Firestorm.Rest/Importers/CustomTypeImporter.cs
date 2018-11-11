using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Firestorm
{
    public class CustomTypeImporter : IRestItemDataImporter
    {
        public bool CanImport(object obj)
        {
            return obj is ICustomTypeDescriptor;
        }

        public Type GetType(object obj)
        {
            return obj.GetType();
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues(object obj)
        {
            var customType = (ICustomTypeDescriptor) obj;
            foreach (PropertyDescriptor propertyDescriptor in customType.GetProperties())
            {
                object value = propertyDescriptor.GetValue(customType);
                yield return new KeyValuePair<string, object>(propertyDescriptor.Name, value);
            }
        }
    }
}