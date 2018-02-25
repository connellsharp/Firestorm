using System.Reflection;
using Firestorm.Endpoints.Start;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.AspNetCore2;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
{
    public static class StemsServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Stems using Stem tpes from the application's entry assembly.
        /// </summary>
        public static IFirestormServicesBuilder AddStems(this IFirestormServicesBuilder builder)
        {
            return builder.AddStems(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem tpes from the given <see cref="Assembly"/>.
        /// </summary>
        public static IFirestormServicesBuilder AddStems(this IFirestormServicesBuilder builder, Assembly assembly)
        {
            return builder.AddStems(assembly, assembly.GetName().Name);
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem tpes from the given <see cref="Assembly"/> within the given <see cref="baseNamespace"/>.
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

            builder.Services.AddSingleton(new AxisTypesLocation<Stem>(assembly, baseNamespace));

            return builder;
        }
    }
}
