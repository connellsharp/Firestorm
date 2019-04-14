using Firestorm.Data;
using System.Data.Entity;

namespace Firestorm.EntityFramework6
{
    public static class EntitiesServicesExtensions
    {
        /// <summary>
        /// Configures a Firestorm Data source for Entity Framework 6.
        /// </summary>
        public static IServicesBuilder AddEntityFramework<TDbContext>(this IServicesBuilder builder)
            where TDbContext : DbContext, new()
        {
            builder.AddDataSource(new EntitiesDataSource<TDbContext>());
            return builder;
        }
    }
}