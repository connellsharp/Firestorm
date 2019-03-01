namespace Firestorm.Fluent
{
    internal static class DataSourceTypeFinderServicesExtensions
    {
        /// <summary>
        /// Configures a <see cref="IItemTypeFinder"/> that finds the entity types in the data source.
        /// </summary>
        internal static IServicesBuilder AddDataSourceTypeFinder(this IServicesBuilder builder)
        {
            builder.Add<IItemTypeFinder, DataSourceTypeFinder>();
            return builder;
        }
    }
}