using Firestorm.AspNetCore2;
using Firestorm.Stems.Roots.DataSource;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
{
    internal static class DataSourceTypeFinderServicesExtensions
    {
        /// <summary>
        /// Configures the <see cref="DataSourceRootResourceFactory"/>.
        /// </summary>
        internal static IFirestormServicesBuilder AddDataSourceTypeFinder(this IFirestormServicesBuilder builder)
        {
            builder.Services.AddSingleton<IItemTypeFinder, DataSourceTypeFinder>();
            return builder;
        }
    }
}