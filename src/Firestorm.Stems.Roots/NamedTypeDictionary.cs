using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Finds types in the loaded assemblies that derive from a given base class and maps them by name.
    /// </summary>
    public class NamedTypeDictionary
    {
        private readonly Dictionary<string, Type> _loadedTypes = new Dictionary<string, Type>(); // todo thread-safe?

        public void AddAll()
        {
            AddTypes(FindAllValidTypes());
        }

        public void AddNamespace([CanBeNull] string @namespace)
        {
            IEnumerable<Type> types = FindAllValidTypes();

            if (@namespace != null)
                types = types.Where(t => t.Namespace != null && t.Namespace.StartsWith(@namespace));

            AddTypes(types);
        }

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

            string name = GetName(type).ToUpperInvariant();
            _loadedTypes.Add(name, type);
        }

        public Type GetType(string name)
        {
            string upperName = name.ToUpperInvariant();

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
         
        protected virtual IEnumerable<Type> FindAllValidTypes()
        {
            // idea from http://stackoverflow.com/a/17680332/369247

            return from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                   where !domainAssembly.FullName.StartsWith("Firestorm.")
                   from assemblyType in domainAssembly.GetExportedTypes()
                   where IsValidType(assemblyType)
                   select assemblyType;
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