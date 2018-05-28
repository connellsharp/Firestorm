using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Gets the types from the given Assembly.
    /// </summary>
    public class AssemblyTypeGetter : ITypeGetter
    {
        private readonly Assembly _assembly;
        private readonly string _baseNamespace;

        public AssemblyTypeGetter(Assembly assembly)
        {
            _assembly = assembly;
            _baseNamespace = null;
        }

        public AssemblyTypeGetter(Assembly assembly, string baseBaseNamespace)
        {
            _assembly = assembly;
            _baseNamespace = baseBaseNamespace;
        }

        public virtual IEnumerable<Type> GetAvailableTypes()
        {
            return from assemblyType in _assembly.GetExportedTypes()
                where _baseNamespace == null || (assemblyType.Namespace != null && assemblyType.Namespace.StartsWith(_baseNamespace))
                select assemblyType;
        }
    }
}