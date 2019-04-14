using System;
using Firestorm.Data;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2
{
    public static class EntitiesServicesExtensions
    {
        /// <summary>
        /// Configures a Firestorm Data source for Entity Framework Core.
        /// </summary>
        public static IServicesBuilder AddEntityFramework<TDbContext>(this IServicesBuilder builder)
            where TDbContext : DbContext
        {
            return builder.AddEntityFramework<TDbContext>(new FirestormEntityOptions());
        }

        /// <summary>
        /// Configures a Firestorm Data source for Entity Framework Core.
        /// </summary>
        public static IServicesBuilder AddEntityFramework<TDbContext>(this IServicesBuilder builder, Action<FirestormEntityOptions> configureAction)
            where TDbContext : DbContext
        {
            var options = new FirestormEntityOptions();
            configureAction(options);
            return builder.AddEntityFramework<TDbContext>(options);
        }

        /// <summary>
        /// Configures a Firestorm Data source for Entity Framework Core.
        /// </summary>
        public static IServicesBuilder AddEntityFramework<TDbContext>(this IServicesBuilder builder, FirestormEntityOptions options)
            where TDbContext : DbContext
        {
            builder.AddDataSource(sp =>
            {
                var requestProvider = sp.GetRequestServiceProvider();
                var dbContextFactory = new EntitiesContextFactory<TDbContext>(requestProvider, options);
                return new EFCoreDataSource<TDbContext>(dbContextFactory);
            });

            return builder;
        }
    }
}