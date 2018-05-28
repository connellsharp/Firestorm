using Firestorm.AspNetCore2;
using Firestorm.Data;
using Firestorm.Stems;
using Microsoft.Extensions.DependencyInjection;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;

namespace Firestorm.Extensions.AspNetCore
{
    internal static class DataSourceRootsServicesExtensions
    {
        /// <summary>
        /// Configures the <see cref="DataSourceRootResourceFactory"/>.
        /// </summary>
        internal static IFirestormServicesBuilder AddDataSourceRoots(this IFirestormServicesBuilder builder)
        {
            builder.Services.AddSingleton<IRootResourceFactory>(sp => new DataSourceRootResourceFactory
            {
                DataSource = sp.GetService<IDataSource>(),
                StemTypeGetter = sp.GetService<AxisTypesLocation<Stem>>().GetTypeGetter(),
                RootBehavior = sp.GetService<DataSourceRootAttributeBehavior>()
            });
            
            return builder;
        }
    }
}