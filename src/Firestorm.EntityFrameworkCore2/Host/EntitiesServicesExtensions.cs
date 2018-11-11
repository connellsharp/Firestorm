using System;
using Firestorm.EntityFrameworkCore2;
using Firestorm.Host;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.Extensions.AspNetCore
{
    public static class EntitiesServicesExtensions
    {
        /// <summary>
        /// Configures a Firestorm Data source for Entity Framework Core.
        /// </summary>
        public static IFirestormServicesBuilder AddEntityFramework<TDbContext>(this IFirestormServicesBuilder builder)
            where TDbContext : DbContext
        {
            return builder.AddEntityFramework<TDbContext>(new FirestormEntityOptions());
        }

        /// <summary>
        /// Configures a Firestorm Data source for Entity Framework Core.
        /// </summary>
        public static IFirestormServicesBuilder AddEntityFramework<TDbContext>(this IFirestormServicesBuilder builder, Action<FirestormEntityOptions> configureAction)
            where TDbContext : DbContext
        {
            var options = new FirestormEntityOptions();
            configureAction(options);
            return builder.AddEntityFramework<TDbContext>(options);
        }

        /// <summary>
        /// Configures a Firestorm Data source for Entity Framework Core.
        /// </summary>
        public static IFirestormServicesBuilder AddEntityFramework<TDbContext>(this IFirestormServicesBuilder builder, FirestormEntityOptions options)
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