using System;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Stems.Roots
{
    public class GlobalTypeGetter : ITypeGetter
    {
        private readonly string _baseNamespace;

        public GlobalTypeGetter()
        {
            _baseNamespace = null;
        }

        public GlobalTypeGetter(string baseBaseNamespace)
        {
            _baseNamespace = baseBaseNamespace;
        }

        public virtual IEnumerable<Type> GetAvailableTypes()
        {
            // idea from http://stackoverflow.com/a/17680332

            return from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                where !domainAssembly.IsDynamic
                where !domainAssembly.FullName.StartsWith("Firestorm.")
                from assemblyType in domainAssembly.GetExportedTypes()
                where _baseNamespace == null || (assemblyType.Namespace != null && assemblyType.Namespace.StartsWith(_baseNamespace))
                select assemblyType;
        }
    }
}