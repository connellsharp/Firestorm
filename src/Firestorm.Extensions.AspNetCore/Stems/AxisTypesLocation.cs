using System.Reflection;
using Firestorm.Stems;
using Firestorm.Stems.Roots;

namespace Firestorm.Extensions.AspNetCore
{
    /// <summary>
    /// Provides a method to get an <see cref="ITypeGetter"/>.
    /// Used to register with dependency injection.
    /// </summary>
    internal class AxisTypesLocation<T>
        where T: IAxis
    {
        public Assembly Assembly { get; }
        public string BaseNamespace { get; }

        internal AxisTypesLocation(Assembly assembly, string baseNamespace)
        {
            Assembly = assembly;
            BaseNamespace = baseNamespace;
        }

        internal ITypeGetter GetTypeGetter()
        {
            return new AssemblyTypeGetter(Assembly, BaseNamespace);
        }
    }
}