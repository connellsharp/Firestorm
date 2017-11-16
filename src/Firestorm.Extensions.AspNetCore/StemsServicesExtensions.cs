using System.Reflection;
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
            return builder.AddStems(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// Configures Firestorm Stems.
        /// </summary>
        public static IFirestormServicesBuilder AddStems(this IFirestormServicesBuilder builder, Assembly assembly)
        {
            return builder.AddStems(assembly, assembly.GetName().Name);
        }

        /// <summary>
        /// Configures Firestorm Stems.
        /// </summary>
        public static IFirestormServicesBuilder AddStems(this IFirestormServicesBuilder builder, Assembly assembly, string baseNamespace)
        {
            builder.Services.AddSingleton<IStartResourceFactory>(sp => new StemsStartResourceFactory
            {
                StemConfiguration = new DefaultStemConfiguration
                {
                    DependencyResolver = new DefaultDependencyResolver(sp)
                },
                RootResourceFactory = sp.GetService<IRootResourceFactory>()
            });

            builder.Services.AddSingleton(new StemTypesLocation(assembly, baseNamespace));

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
                var stemTypesLocation = sp.GetService<StemTypesLocation>();

                return new DataSourceRootResourceFactory
                {
                    DataSource = new EFCoreDataSource<TDbContext>(() => contextAccessor.HttpContext.RequestServices.GetService<TDbContext>()),
                    StemTypeGetter = new AssemblyTypeGetter(stemTypesLocation.Assembly, stemTypesLocation.BaseNamespace)
                };
            });

            return builder;
        }
    }
}
