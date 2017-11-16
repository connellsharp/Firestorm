using System.Reflection;

namespace Firestorm.Extensions.AspNetCore
{
    internal class StemTypesLocation
    {
        public Assembly Assembly { get; }
        public string BaseNamespace { get; }

        internal StemTypesLocation(Assembly assembly, string baseNamespace)
        {
            Assembly = assembly;
            BaseNamespace = baseNamespace;
        }
    }
}