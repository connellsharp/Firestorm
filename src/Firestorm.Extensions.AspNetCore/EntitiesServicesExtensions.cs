using Firestorm.Data;
using Firestorm.Data.EFCore2;
using Firestorm.Endpoints.AspNetCore;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            builder.Services.AddSingleton<IDataSource>(sp => new EFCoreDataSource<TDbContext>(new RequestServiceProvider(sp)));

            builder.Services.AddSingleton<IRootResourceFactory>(sp => new DataSourceRootResourceFactory
                {
                    DataSource = sp.GetService<IDataSource>(),
                    StemTypeGetter = sp.GetService<StemTypesLocation>().GetTypeGetter()
                });

            return builder;
        }
    }
}