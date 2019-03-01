using System;

namespace Firestorm.Data
{
    public static class DataSourceServicesExtensions
    {
        /// <summary>
        /// Configures the data source for this Firestorm API.
        /// </summary>
        public static IServicesBuilder AddDataSource(this IServicesBuilder builder, IDataSource dataSource)
        {
            //builder.AddDataSourceTypeFinder();
            //builder.AddDataSourceRoots();
            
            builder.Add<IDataSource>(dataSource);
            return builder;
        }

        /// <summary>
        /// Configures the data source for this Firestorm API.
        /// </summary>
        public static IServicesBuilder AddDataSource(this IServicesBuilder builder, Func<IServiceProvider, IDataSource> dataSourceFunc)
        {
            //builder.AddDataSourceTypeFinder();
            //builder.AddDataSourceRoots();
            
            builder.Add<IDataSource>(dataSourceFunc);
            return builder;
        }
    }
}