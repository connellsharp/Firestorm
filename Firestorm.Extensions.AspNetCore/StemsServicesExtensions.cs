using Firestorm.Endpoints.Start;
using Firestorm.Engine.EFCore2;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;
using Microsoft.EntityFrameworkCore;
using Firestorm.Endpoints.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
{
    public static class StemsServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Stems.
        /// </summary>
        public static IFirestormServicesBuilder AddStems(this IFirestormServicesBuilder builder)
        {
            builder.Services.AddSingleton<IStartResourceFactory>(sp => new StemsStartResourceFactory
            {
                StemConfiguration = new DefaultStemConfiguration
                {
                    DependencyResolver = new DefaultDependencyResolver(sp)
                },
                RootResourceFactory = sp.GetService<IRootResourceFactory>()
            });

            return builder;
        }

        /// <summary>
        /// Configures Firestorm Stems.
        /// </summary>
        public static IFirestormServicesBuilder AddEntityFramework<TDbContext>(this IFirestormServicesBuilder builder)
            where TDbContext : DbContext
        {
            builder.Services.AddSingleton<IRootResourceFactory>(sp =>
            {
                var contextAccessor = sp.GetService<IHttpContextAccessor>();

                return new DataSourceRootResourceFactory
                {
                    DataSource = new EFCoreDataSource<TDbContext>(() => contextAccessor.HttpContext.RequestServices.GetService<TDbContext>()),
                    StemsNamespace = "TestingFirestorm.AspNetCore.Stems",
                };
            });

            return builder;
        }
    }
}
