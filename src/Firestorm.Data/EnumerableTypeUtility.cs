using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm.Data
{
    public static class EnumerableTypeUtility
    {
        public static bool IsEnumerable(Type type)
        {
            return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static Type GetItemType(Type type)
        {
            Type enumerableType = type.GetGenericInterface(typeof(IEnumerable<>));

            if (enumerableType == null)
                throw new ArgumentException("Given type does not inherit IEnumerable<>.", nameof(type));

            var itemType = enumerableType.GetGenericArguments()[0];

            return itemType;
        }

        public static IList CreateList(Type itemType)
        {
            Type listType = typeof(List<>).MakeGenericType(itemType);
            var newList = (IList) Activator.CreateInstance(listType);
            return newList;
        }


        // TODO Exact duplicates of these in Firestorm.Stems.Attributes.TypeExtensions

        [CanBeNull]
        private static Type GetGenericInterface(this Type type, Type genericInterfaceDefinition)
        {
            if (type.IsOfGenericTypeDefinition(genericInterfaceDefinition))
                return type;

            return type.GetInterfaces().FirstOrDefault(t => t.IsOfGenericTypeDefinition(genericInterfaceDefinition));
        }

        private static bool IsOfGenericTypeDefinition(this Type type, Type genericInterfaceDefinition)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericInterfaceDefinition;
        }
    }
}
