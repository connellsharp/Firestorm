using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Firestorm.AspNetCore2
{

    public static class FirestormServicesExtensions
    {
        /// <summary>
        /// Adds Firestorm services and configures them using <see cref="IOptions{TOptions}"/> and the given action.
        /// </summary>
        public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services, RestEndpointConfiguration endpointConfiguration)
        {
            return services.AddFirestorm()
                .AddRequiredFirestormServices()
                .ConfigureEndpoints(endpointConfiguration);
        }

        /// <summary>
        /// Adds Firestorm services and configures them using <see cref="IOptions{RestEndpointConfiguration}"/> and the given action.
        /// </summary>
        public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services, Action<RestEndpointConfiguration> configureEndpointsAction)
        {
            return services.AddFirestorm()
                .AddRequiredFirestormServices()
                .ConfigureEndpoints(configureEndpointsAction);
        }

        /// <summary>
        /// Configures Firestorm using <see cref="IOptions{RestEndpointConfiguration}"/> with the given action.
        /// </summary>
        public static IFirestormServicesBuilder ConfigureEndpoints(this IFirestormServicesBuilder builder, RestEndpointConfiguration endpointConfiguration)
        {
            builder.Add(endpointConfiguration);
            return builder;
        }

        /// <summary>
        /// Configures Firestorm using <see cref="IOptions{RestEndpointConfiguration}"/> with the given action.
        /// </summary>
        public static IFirestormServicesBuilder ConfigureEndpoints(this IFirestormServicesBuilder builder, Action<RestEndpointConfiguration> configureEndpointsAction)
        {
            builder.Services.Configure<DefaultRestEndpointConfiguration>(configureEndpointsAction);
            builder.Add<RestEndpointConfiguration>(sp => sp.GetService<IOptions<DefaultRestEndpointConfiguration>>().Value);
            return builder;
        }

        /// <summary>
        /// Adds Firestorm services.
        /// </summary>
        private static IFirestormServicesBuilder AddRequiredFirestormServices(this IFirestormServicesBuilder builder)
        {
            builder.Services.AddOptions();

            builder.Add<IHttpContextAccessor, HttpContextAccessor>();

            builder.Add<FirestormConfiguration>(sp => new FirestormConfiguration
            {
                EndpointConfiguration = sp.GetService<RestEndpointConfiguration>() ?? new DefaultRestEndpointConfiguration(),
                StartResourceFactory = sp.GetService<IStartResourceFactory>()
            });

            return builder;
        }
    }
}