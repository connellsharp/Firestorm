using System;
using Firestorm.AspNetCore2;
using Firestorm.Data;
using Firestorm.Stems;
using Microsoft.Extensions.DependencyInjection;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;

namespace Firestorm.Extensions.AspNetCore
{
    public static class DataSourceServicesExtensions
    {
        /// <summary>
        /// Configures the data source for this Firestorm API.
        /// </summary>
        public static IFirestormServicesBuilder AddDataSource(this IFirestormServicesBuilder builder, IDataSource dataSource)
        {
            builder.AddDataSourceTypeFinder();
            builder.AddDataSourceRoots();
            
            builder.Services.AddSingleton<IDataSource>(dataSource);
            return builder;
        }

        /// <summary>
        /// Configures the data source for this Firestorm API.
        /// </summary>
        public static IFirestormServicesBuilder AddDataSource(this IFirestormServicesBuilder builder, Func<IServiceProvider, IDataSource> dataSourceFunc)
        {
            builder.AddDataSourceTypeFinder();
            builder.AddDataSourceRoots();
            
            builder.Services.AddSingleton<IDataSource>(dataSourceFunc);
            return builder;
        }

        /// <summary>
        /// Configures the <see cref="DataSourceRootResourceFactory"/>.
        /// </summary>
        private static IFirestormServicesBuilder AddDataSourceRoots(this IFirestormServicesBuilder builder)
        {
            builder.Services.AddSingleton<IRootResourceFactory>(sp => new DataSourceRootResourceFactory
            {
                DataSource = sp.GetService<IDataSource>(),
                StemTypeGetter = sp.GetService<AxisTypesLocation<Stem>>().GetTypeGetter()
            });
            
            return builder;
        }

        /// <summary>
        /// Configures the <see cref="DataSourceRootResourceFactory"/>.
        /// </summary>
        private static IFirestormServicesBuilder AddDataSourceTypeFinder(this IFirestormServicesBuilder builder)
        {
            builder.Services.AddSingleton<IItemTypeFinder, DataSourceTypeFinder>();
            return builder;
        }
    }
}