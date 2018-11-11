using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceProviderServiceExtensions = Firestorm.Host.ServiceProviderServiceExtensions;

namespace Firestorm.AspNetCore2
{
    public static class FirestormMiddlewareConfigExtensions
    {
        /// <summary>
        /// Configures Firestorm using the services and options configured in the app.
        /// </summary>
        public static IApplicationBuilder UseFirestorm(this IApplicationBuilder app, RestEndpointConfiguration endpointConfiguration)
        {
            var config = new FirestormConfiguration
            {
                EndpointConfiguration = endpointConfiguration,
                StartResourceFactory = app.GetScopedService<IStartResourceFactory>()
            };
            
            app.UseFirestorm(config);
            return app;
        }

        /// <summary>
        /// Configures Firestorm by executing the <see cref="configureAction"/> on a new instance of <see cref="FirestormConfiguration"/>.
        /// </summary>
        public static IApplicationBuilder UseFirestorm(this IApplicationBuilder app, Action<FirestormConfiguration> configureAction)
        {
            FirestormConfiguration config = LoadConfigurationAsService(app);
            configureAction(config);

            app.UseFirestorm(config);
            return app;
        }

        /// <summary>
        /// Configures Firestorm using the given <see cref="configuration"/> object.
        /// </summary>
        public static IApplicationBuilder UseFirestorm(this IApplicationBuilder app, FirestormConfiguration configuration)
        {
            configuration.EnsureValid();

            app.UseFirestorm();
                
                ap
                .AddStartResoureFactory(configuration.StartResourceFactory)
                .AddEndpoints(configuration.EndpointConfiguration);
            
            return app;
        }

        private static FirestormConfiguration LoadConfigurationAsService(IApplicationBuilder app)
        {
            var config = app.GetScopedService<FirestormConfiguration>() ??
                         app.GetScopedService<IOptions<FirestormConfiguration>>()?.Value ??
                         new FirestormConfiguration();

            if (config.StartResourceFactory == null)
                config.StartResourceFactory = app.GetScopedService<IStartResourceFactory>();

            return config;
        }

        private static TService GetScopedService<TService>(this IApplicationBuilder app)
        {
            var scopeFactory = ServiceProviderServiceExtensions.GetService<IServiceScopeFactory>(app.ApplicationServices);

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                return ServiceProviderServiceExtensions.GetService<TService>(services);
            }
        }
    }
}
