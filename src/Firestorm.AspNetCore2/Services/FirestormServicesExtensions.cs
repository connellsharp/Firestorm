using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Firestorm.AspNetCore2
{
    public static class FirestormServicesExtensions
    {
        /// <summary>
        /// Adds Firestorm services.
        /// </summary>
        public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services)
        {
            IFirestormServicesBuilder builder = new FirestormServicesBuilder(services);
            builder.AddRequiredFirestormServices();
            return builder;
        }

        /// <summary>
        /// Adds Firestorm services and configures them using <see cref="IOptions{RestEndpointConfiguration}"/> and the given action.
        /// </summary>
        public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services, RestEndpointConfiguration endpointConfiguration)
        {
            return services.AddFirestorm()
                .ConfigureEndpoints(endpointConfiguration);
        }

        /// <summary>
        /// Adds Firestorm services and configures them using <see cref="IOptions{RestEndpointConfiguration}"/> and the given action.
        /// </summary>
        public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services, Action<RestEndpointConfiguration> configureEndpointsAction)
        {
            return services.AddFirestorm()
                .ConfigureEndpoints(configureEndpointsAction);
        }

        /// <summary>
        /// Configures Firestorm using <see cref="IOptions{RestEndpointConfiguration}"/> with the given action.
        /// </summary>
        public static IFirestormServicesBuilder ConfigureEndpoints(this IFirestormServicesBuilder builder, RestEndpointConfiguration endpointConfiguration)
        {
            builder.Services.AddSingleton(endpointConfiguration);
            return builder;
        }

        /// <summary>
        /// Configures Firestorm using <see cref="IOptions{RestEndpointConfiguration}"/> with the given action.
        /// </summary>
        public static IFirestormServicesBuilder ConfigureEndpoints(this IFirestormServicesBuilder builder, Action<RestEndpointConfiguration> configureEndpointsAction)
        {
            builder.Services.Configure<DefaultRestEndpointConfiguration>(configureEndpointsAction);
            builder.Services.AddSingleton<RestEndpointConfiguration>(sp => sp.GetService<IOptions<DefaultRestEndpointConfiguration>>().Value);
            return builder;
        }

        /// <summary>
        /// Configures the start resource for this Firestorm API.
        /// </summary>
        public static IFirestormServicesBuilder AddStartResourceFactory(this IFirestormServicesBuilder builder, IStartResourceFactory startResourceFactory)
        {
            builder.Services.AddSingleton<IStartResourceFactory>(startResourceFactory);
            return builder;
        }

        /// <summary>
        /// Configures the start resource for this Firestorm API.
        /// </summary>
        public static IFirestormServicesBuilder AddStartResourceFactory(this IFirestormServicesBuilder builder, Func<IServiceProvider,IStartResourceFactory> startResourceFactoryFunc)
        {
            builder.Services.AddSingleton<IStartResourceFactory>(startResourceFactoryFunc);
            return builder;
        }

        /// <summary>
        /// Adds Firestorm services.
        /// </summary>
        private static IFirestormServicesBuilder AddRequiredFirestormServices(this IFirestormServicesBuilder builder)
        {
            builder.Services.AddOptions();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddSingleton<FirestormConfiguration>(sp => new FirestormConfiguration
            {
                EndpointConfiguration = sp.GetService<RestEndpointConfiguration>() ?? new DefaultRestEndpointConfiguration(),
                StartResourceFactory = sp.GetService<IStartResourceFactory>()
            });

            return builder;
        }
    }
}
