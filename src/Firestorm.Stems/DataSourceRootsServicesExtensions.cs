using System;
using Firestorm.Data;
using Firestorm.Host;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;

namespace Firestorm.Stems
{
    [Obsolete("Automatically detected in AddStems now")]
    internal static class DataSourceRootsServicesExtensions
    {
        /// <summary>
        /// Configures the <see cref="DataSourceRootResourceFactory"/> for Stems.
        /// </summary>
        internal static IFirestormServicesBuilder AddDataSourceRoots(this IFirestormServicesBuilder builder)
        {
            builder.Add<IRootResourceFactory>(sp => new DataSourceRootResourceFactory
            {
                DataSource = sp.GetService<IDataSource>(),
                StemTypeGetter = sp.GetService<AxisTypesLocation<Stem>>().GetTypeGetter(),
                RootBehavior = DataSourceRootAttributeBehavior.UseAllStemsExceptDisallowed // TODO support this option
            });
            
            return builder;
        }
    }
}