using System;
using System.Reflection;
using Firestorm.Data;
using Firestorm.Host;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Stems.Roots.Derive;

namespace Firestorm.Stems
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
                StemsServices = new StemsServices
                {
                    DependencyResolver = new DefaultDependencyResolver(sp.GetRequestServiceProvider()),
                    AutoPropertyMapper = sp.GetService<IPropertyAutoMapper>() ?? new DefaultPropertyAutoMapper()
                },
                RootResourceFactory = CreateRootResourceFactory(sp)
            });

            builder.Add(new AxisTypesLocation<Stem>(assembly, baseNamespace));

            return builder;
        }

        private static IRootResourceFactory CreateRootResourceFactory(IServiceProvider sp)
        {
            var dataSource = sp.GetService<IDataSource>();
            if (dataSource != null)
            {
                return new DataSourceRootResourceFactory
                {
                    StemTypeGetter = sp.GetService<ITypeGetter>()
                                  ?? sp.GetService<AxisTypesLocation<Stem>>().GetTypeGetter(),
                    DataSource = dataSource,
                    RootBehavior = DataSourceRootAttributeBehavior.UseAllStemsExceptDisallowed // TODO support option
                };
            }
            else
            {
                return new DerivedRootsResourceFactory
                {
                    RootTypeGetter = sp.GetService<AxisTypesLocation<Root>>().GetTypeGetter()
                };
            }
        }
    }
}
