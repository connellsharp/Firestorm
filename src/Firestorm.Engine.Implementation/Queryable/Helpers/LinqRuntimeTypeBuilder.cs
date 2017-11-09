using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Firestorm.Engine.Queryable
{
    /// <summary>
    /// Taken and refactored from http://stackoverflow.com/a/723018/369247
    /// </summary>
    internal static class LinqRuntimeTypeBuilder
    {
        private static readonly AssemblyName AssemblyName = new AssemblyName { Name = "DynamicLinqTypes" };
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly Dictionary<string, Type> BuiltTypes = new Dictionary<string, Type>();

        static LinqRuntimeTypeBuilder()
        {
            ModuleBuilder = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(AssemblyName.Name);
        }

        public static Type GetDynamicType(IEnumerable<KeyValuePair<string, Type>> fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            //if (!fields.Any()) throw new ArgumentOutOfRangeException(nameof(fields), "fields must have at least 1 field definition");

            lock (BuiltTypes)
            {
                string typeKey = GetTypeKey(fields);

                if (BuiltTypes.ContainsKey(typeKey))
                    return BuiltTypes[typeKey];

                TypeBuilder typeBuilder = ModuleBuilder.DefineType(typeKey, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                foreach (var field in fields)
                    typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);

                BuiltTypes[typeKey] = typeBuilder.CreateTypeInfo();

                return BuiltTypes[typeKey];
            }
        }

        public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
        {
            return GetDynamicType(fields.Select(f => new KeyValuePair<string, Type>(f.Name, f.PropertyType)));
        }

        private static string GetTypeKey(IEnumerable<KeyValuePair<string, Type>> fields)
        {
            //TODO: optimize the type caching -- if fields are simply reordered, that doesn't mean that they're actually different types, so this needs to be smarter
            var builder = new StringBuilder("LinqRuntimeType");

            foreach (var field in fields)
            {
                builder.AppendFormat(";{0} {1}", field.Value.Name, field.Key);
            }

            return builder.ToString();
        }
    }
}