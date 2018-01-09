using System;
using JetBrains.Annotations;

namespace Firestorm.Fluent.Sources
{
    internal static class TypeExtensions
    {
        [CanBeNull]
        public static Type GetGenericSubclass(this Type type, Type genericSubclass)
        {
            // TODO Exact duplicate of this in Stems.TypeExceptions.

            while (type != null && type != typeof(object))
            {
                Type genericDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

                if (genericDefinition == genericSubclass)
                    return type;

                type = type.BaseType;
            }

            return null;
        }
    }
}