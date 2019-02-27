using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firestorm.Endpoints.Directories
{
    /// <summary>
    /// The root directory when using Endpoints alone.
    /// </summary>
    public class AttributeBasedDirectory : DictionaryDirectory
    {
        public void LoadFromAttributes()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                IEnumerable<DirectoryTypePair> types = TryGetAssemblyTypes(assembly);

                if (types == null)
                    continue;

                foreach (var tuple in types)
                {
                    Add(tuple.DirectoryName, tuple.Type);
                }
            }
        }

        private static IEnumerable<DirectoryTypePair> TryGetAssemblyTypes(Assembly assembly)
        {
            try
            {
                return from type in assembly.GetExportedTypes()
                       from startAttribute in type.GetCustomAttributes<RestStartResourceAttribute>(true)
                       select new DirectoryTypePair
                       {
                           DirectoryName = startAttribute.DirectoryName,
                           Type = type
                       };
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }

        private class DirectoryTypePair
        {
            public string DirectoryName { get; set; }
            public Type Type { get; set; }
        }
    }
}