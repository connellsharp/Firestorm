using System;
using Firestorm.Data;
using Firestorm.Host;

namespace Firestorm.Extensions.AspNetCore
{
    public static class DataSourceServicesExtensions
    {
        /// <summary>
        /// Configures the data source for this Firestorm API.
        /// </summary>
        public static IFirestormServicesBuilder AddDataSource(this IFirestormServicesBuilder builder, IDataSource dataSource)
        {
            //builder.AddDataSourceTypeFinder();
            //builder.AddDataSourceRoots();
            
            builder.Add<IDataSource>(dataSource);
            return builder;
        }

        /// <summary>
        /// Configures the data source for this Firestorm API.
        /// </summary>
        public static IFirestormServicesBuilder AddDataSource(this IFirestormServicesBuilder builder, Func<IFirestormServiceProvider, IDataSource> dataSourceFunc)
        {
            //builder.AddDataSourceTypeFinder();
            //builder.AddDataSourceRoots();
            
            builder.Add<IDataSource>(dataSourceFunc);
            return builder;
        }
    }
}