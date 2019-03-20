using System;
using System.Reflection;
using Firestorm.Data;
using Firestorm.Features;
using Firestorm.Host;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Combined;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Stems.Roots.Derive;

namespace Firestorm.Stems
{
    public static class StemsServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Stems using Stem types from the application's entry assembly.
        /// </summary>
        public static IServicesBuilder AddStems(this IServicesBuilder builder)
        {
            return builder.AddStems(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem types from the given <see cref="Assembly"/>.
        /// </summary>
        public static IServicesBuilder AddStems(this IServicesBuilder builder, Assembly assembly)
        {
            return builder.AddStems(assembly, assembly.GetName().Name);
        }

        /// <summary>
        /// Configures Firestorm Stems using Stem types from the given <see cref="Assembly"/> within the given <see cref="baseNamespace"/>.
        /// </summary>
        public static IServicesBuilder AddStems(this IServicesBuilder builder, Assembly assembly, string baseNamespace)
        {
            return builder.AddStems(new StemsConfiguration
            {
                Assembly = assembly,
                BaseNamespace = baseNamespace
            });
        }

        /// <summary>
        /// Configures Firestorm Stems using the given <see cref="StemsConfiguration"/>.
        /// </summary>
        public static IServicesBuilder AddStems(this IServicesBuilder builder, StemsConfiguration configuration)
        {
            builder.AddWithFeatures<StemsServices>()
                .AddFeature<StemsServices, DefaultStemsFeature>();
            
            builder.Add<IRootStartInfoFactory>(CreateRootResourceFactory);
            builder.Add(new AxisTypesLocation<Stem>(configuration.Assembly, configuration.BaseNamespace));
            
            builder.AddStartResourceFactory(sp => new StemsStartResourceFactory(sp.GetService<StemsServices>(), sp.GetService<IRootStartInfoFactory>()));

            return builder;
        }

        private static IRootStartInfoFactory CreateRootResourceFactory(IServiceProvider sp)
        {
            var dataSource = sp.GetService<IDataSource>();
            if (dataSource != null)
            {
                var typeGetter = sp.GetService<ITypeGetter>()
                                 ?? sp.GetService<AxisTypesLocation<Stem>>().GetTypeGetter();

                var rootBehaviour = DataSourceRootAttributeBehavior.UseAllStemsExceptDisallowed; // TODO support option
                
                return new DataSourceVaseStartInfoFactory(dataSource, typeGetter, rootBehaviour);
            }
            else
            {
                ITypeGetter rootTypeGetter = sp.GetService<AxisTypesLocation<Root>>()?.GetTypeGetter() ??
                                             sp.GetService<AxisTypesLocation<Stem>>().GetTypeGetter();

                return new DerivedRootStartInfoFactory(rootTypeGetter);
            }
        }
    }
}
