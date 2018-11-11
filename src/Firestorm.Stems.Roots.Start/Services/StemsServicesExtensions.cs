using System.Reflection;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.Host;

namespace Firestorm.Extensions.AspNetCore
{
    public static class StemsServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Stems using Stem types from the application's entry assembly.
        /// </summary>
        public static IFirestormServicesBuilder AddStems(this IFirestormServicesBuilder builder)
        {
            return builder.AddStems(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem types from the given <see cref="Assembly"/>.
        /// </summary>
        public static IFirestormServicesBuilder AddStems(this IFirestormServicesBuilder builder, Assembly assembly)
        {
            return builder.AddStems(assembly, assembly.GetName().Name);
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem types from the given <see cref="Assembly"/> within the given <see cref="baseNamespace"/>.
        /// </summary>
        public static IFirestormServicesBuilder AddStems(this IFirestormServicesBuilder builder, Assembly assembly, string baseNamespace)
        {
            builder.AddStartResourceFactory(sp => new StemsStartResourceFactory
            {
                StemConfiguration = new DefaultStemConfiguration
                {
                    DependencyResolver = new DefaultDependencyResolver(new RequestServiceProvider(sp))
                },
                RootResourceFactory = sp.GetService<IRootResourceFactory>()
            });

            builder.Add(new AxisTypesLocation<Stem>(assembly, baseNamespace));

            return builder;
        }
    }
}
