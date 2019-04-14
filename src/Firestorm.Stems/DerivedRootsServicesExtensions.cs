using System;
using System.Reflection;
using Firestorm.Host;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Derive;

namespace Firestorm.Stems
{
    [Obsolete("Automatically detected in AddStems now")]
    public static class DerivedRootsServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Stems using Stem types from the application's entry assembly.
        /// </summary>
        public static IServicesBuilder AddRoots(this IServicesBuilder builder)
        {
            return builder.AddRoots(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem types from the given <see cref="Assembly"/>.
        /// </summary>
        public static IServicesBuilder AddRoots(this IServicesBuilder builder, Assembly assembly)
        {
            return builder.AddRoots(assembly, assembly.GetName().Name);
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem types from the given <see cref="Assembly"/> within the given <see cref="baseNamespace"/>.
        /// </summary>
        public static IServicesBuilder AddRoots(this IServicesBuilder builder, Assembly assembly, string baseNamespace)
        {
            builder.Add(new AxisTypesLocation<Root>(assembly, baseNamespace));
            return builder;
        }
    }
}
