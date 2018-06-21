using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Finds types in the loaded assemblies that derive from a given base class and maps them by name.
    /// </summary>
    public class NamedTypeDictionary
    {
        private readonly Dictionary<string, Type> _loadedTypes = new Dictionary<string, Type>(); // todo thread-safe?

        public void AddTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                AddType(type);
            }
        }

        public void AddType(Type type)
        {
            string key = GetKey(GetName(type));
            _loadedTypes.Add(key, type);
        }

        public Type GetType(string name)
        {
            string upperName = GetKey(name);

            if (!_loadedTypes.ContainsKey(upperName))
                throw new KeyNotFoundException("A named type with the name '" + name + "' cannot be found in this dictionary.");

            return _loadedTypes[upperName];
        }

        public IEnumerable<string> GetAllNames()
        {
            return _loadedTypes.Keys;
        }

        public IEnumerable<Type> GetAllTypes()
        {
            return _loadedTypes.Values;
        }

        protected virtual string GetKey(string name)
        {
            return name; //.ToUpperInvariant();
        }

        protected virtual string GetName(Type type)
        {
            return type.Name;
        }
    }
}