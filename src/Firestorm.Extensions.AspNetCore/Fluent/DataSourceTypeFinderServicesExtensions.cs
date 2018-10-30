using Firestorm.AspNetCore2;
using Firestorm.Stems.Roots.DataSource;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
{
    internal static class DataSourceTypeFinderServicesExtensions
    {
        /// <summary>
        /// Configures a <see cref="IItemTypeFinder"/> that finds the entity types in the data source.
        /// </summary>
        internal static IFirestormServicesBuilder AddDataSourceTypeFinder(this IFirestormServicesBuilder builder)
        {
            builder.Add<IItemTypeFinder, DataSourceTypeFinder>();
            return builder;
        }
    }
}