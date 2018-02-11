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
            if (!IsValidType(type))
                throw new StemStartSetupException(type.Name + " is not a valid type to use in this dictionary.");

            AddTypeInternal(type);
        }

        public void AddVaidTypes(ITypeGetter typeGetter)
        {
            foreach (Type type in typeGetter.GetAvailableTypes())
            {
                if (IsValidType(type))
                    AddTypeInternal(type);
            }
        }

        private void AddTypeInternal(Type type)
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

        protected virtual bool IsValidType(Type type)
        {
            return true;
        }
    }
}