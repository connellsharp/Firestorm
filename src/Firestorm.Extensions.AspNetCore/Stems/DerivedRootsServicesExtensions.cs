using System.Reflection;
using Firestorm.Endpoints.Start;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.AspNetCore2;
using Firestorm.Data;
using Firestorm.Stems.Roots.Derive;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
{
    public static class DerivedRootsServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Stems using Stem tpes from the application's entry assembly.
        /// </summary>
        public static IFirestormServicesBuilder AddRoots(this IFirestormServicesBuilder builder)
        {
            return builder.AddRoots(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem tpes from the given <see cref="Assembly"/>.
        /// </summary>
        public static IFirestormServicesBuilder AddRoots(this IFirestormServicesBuilder builder, Assembly assembly)
        {
            return builder.AddRoots(assembly, assembly.GetName().Name);
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem tpes from the given <see cref="Assembly"/> within the given <see cref="baseNamespace"/>.
        /// </summary>
        public static IFirestormServicesBuilder AddRoots(this IFirestormServicesBuilder builder, Assembly assembly, string baseNamespace)
        {
            builder.AddDerivedRoots();       

            builder.Services.AddSingleton(new AxisTypesLocation<Root>(assembly, baseNamespace));
            return builder;
        }

        /// <summary>
        /// Configures the <see cref="DerivedRootsResourceFactory"/>.
        /// </summary>
        private static IFirestormServicesBuilder AddDerivedRoots(this IFirestormServicesBuilder builder)
        {
            builder.Services.AddSingleton<IRootResourceFactory>(sp => new DerivedRootsResourceFactory
            {
                RootTypeGetter = sp.GetService<AxisTypesLocation<Root>>().GetTypeGetter()
            });
            
            return builder;
        }
    }
}
