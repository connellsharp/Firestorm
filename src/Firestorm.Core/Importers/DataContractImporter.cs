using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Firestorm
{
    /// <summary>
    /// Imports types with the <see cref="DataContractAttribute"/>.
    /// </summary>
    /// <remarks>
    /// Consider moving to another library as this is only used in the client so far and is the only class that depends on <see cref="System.Runtime.Serialization"/>.
    /// </remarks>
    public class DataContractImporter : IRestItemDataImporter
    {
        public bool CanImport(object obj)
        {
            Type type = obj.GetType();
            return Attribute.IsDefined(type, typeof(DataContractAttribute));
        }

        public Type GetType(object obj)
        {
            return obj.GetType();
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues(object obj)
        {
            Type type = obj.GetType();
            const BindingFlags instancePublicAndNot = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (PropertyInfo propertyInfo in type.GetProperties(instancePublicAndNot))
            {
                var dataMemberAttribute = propertyInfo.GetCustomAttribute<DataMemberAttribute>();
                if (dataMemberAttribute == null)
                    continue;

                object value = propertyInfo.GetValue(obj);
                yield return new KeyValuePair<string, object>(dataMemberAttribute.Name, value);
            }

            foreach (FieldInfo fieldInfo in type.GetFields(instancePublicAndNot))
            {
                var dataMemberAttribute = fieldInfo.GetCustomAttribute<DataMemberAttribute>();
                if (dataMemberAttribute == null)
                    continue;

                object value = fieldInfo.GetValue(obj);
                yield return new KeyValuePair<string, object>(dataMemberAttribute.Name, value);
            }
        }
    }
}