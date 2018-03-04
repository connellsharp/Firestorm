using System;
using Firestorm.AspNetCore2;
using Firestorm.Data;
using Firestorm.Stems;
using Microsoft.Extensions.DependencyInjection;
using Firestorm.Stems.Roots;

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
    }
}