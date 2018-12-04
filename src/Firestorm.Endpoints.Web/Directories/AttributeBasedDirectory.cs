using System;
using System.Reflection;

namespace Firestorm.Endpoints.Web
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
                try
                {
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        foreach (var startupAttribute in type.GetCustomAttributes<RestStartResourceAttribute>(true))
                        {
                            Add(startupAttribute.DirectoryName, type);
                        }
                    }
                }
                catch (NotSupportedException)
                {
                }
            }
        }
    }
}