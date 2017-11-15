using System;
using System.Linq;
using Firestorm.Endpoints.Start;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Endpoints.AspNetCore
{
    public static class FirestormMiddlewareExtensions
    {
        /// <summary>
        /// Configures Firestorm using the services and options configured in the app.
        /// </summary>
        public static IApplicationBuilder UseFirestorm(this IApplicationBuilder app)
        {
            var config = app.GetScopedService<FirestormConfiguration>();

            //app.UseMiddleware<FirestormMiddleware>(); // this should mean the IoC creates a new config object.
            app.UseFirestorm(config);
            return app;
        }

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

            //app.UseMiddleware<FirestormMiddleware>(); // this should mean the IoC creates a new config object.
            app.UseFirestorm(config);
            return app;
        }

        /// <summary>
        /// Configures Firestorm by executing the <see cref="configureAction"/> on a new instance of <see cref="FirestormConfiguration"/>.
        /// </summary>
        public static IApplicationBuilder UseFirestorm(this IApplicationBuilder app, Action<FirestormConfiguration> configureAction)
        {
            var config = app.GetScopedService<FirestormConfiguration>() ?? new FirestormConfiguration();
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

            app.UseMiddleware<FirestormMiddleware>(configuration);
            return app;
        }

        private static TService GetScopedService<TService>(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                return services.GetService<TService>();
            }
        }
    }
}
