using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm.Stems.Attributes
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> WhereTypeOfGenericInterface(this IEnumerable<Type> types, Type genericInterfaceDefinition)
        {
            foreach (Type type in types)
            {
                if (GetGenericInterface(type, genericInterfaceDefinition) != null)
                    yield return type;
            }
        }

        public static bool InheritsGenericInterface(this Type type, Type genericInterfaceDefinition)
        {
            return GetGenericInterface(type, genericInterfaceDefinition) != null;
        }
        
        [CanBeNull]
        public static Type GetGenericInterface(this Type type, Type genericInterfaceDefinition)
        {
            // TODO Exact duplicate of this in Firestorm.Data.EnumerableTypeUtility

            if (type.IsOfGenericTypeDefinition(genericInterfaceDefinition))
                return type;

            return type.GetInterfaces().FirstOrDefault(t => t.IsOfGenericTypeDefinition(genericInterfaceDefinition));
        }

        public static bool IsOfGenericTypeDefinition(this Type type, Type genericInterfaceDefinition)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericInterfaceDefinition;
        }

        [CanBeNull]
        public static Type GetGenericSubclass(this Type type, Type genericSubclass)
        {
            while (type != null && type != typeof(object))
            {
                Type genericDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

                if (genericDefinition == genericSubclass)
                    return type;

                type = type.BaseType;
            }

            return null;
        }
        
        public static bool IsSubclassOfGeneric(this Type type, Type genericSubclass)
        {
            return GetGenericSubclass(type, genericSubclass) != null;
        }

        public static bool InheritsInterface(this Type type, Type interfaceType)
        {
            return type.GetInterface(interfaceType.Name) != null;
        }

        // TODO exact same method appears in Engine.ScalarFieldHelper
        public static bool IsScalarType(this Type fieldType)
        {
            Type type = Nullable.GetUnderlyingType(fieldType) ?? fieldType;
            TypeCode typeCode = Type.GetTypeCode(type);
            return typeCode != TypeCode.Empty && typeCode != TypeCode.Object;
        }
    }
}
