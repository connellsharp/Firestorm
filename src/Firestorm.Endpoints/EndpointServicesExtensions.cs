using System;
using Firestorm.Features;
using Firestorm.Host;
using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    [PublicAPI]
    public static class EndpointServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm endpoints.
        /// Assumes <see cref="EndpointConfiguration"/> is already configured elsewhere.
        /// </summary>
        public static IServicesBuilder AddEndpointsInvoker(this IServicesBuilder builder)
        {
            builder
                .Add<IRequestInvoker, EndpointsRequestInvoker>()
                .AddWithFeatures<EndpointServices>()
                .AddFeature<EndpointServices, DefaultEndpointFeature>();

            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints.
        /// </summary>
        public static IServicesBuilder AddEndpoints(this IServicesBuilder builder, EndpointConfiguration config)
        {
            builder.AddEndpointsInvoker();
            
            builder.Add<EndpointConfiguration>(config);
            
            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints.
        /// </summary>
        public static IServicesBuilder AddEndpoints(this IServicesBuilder builder, Action<EndpointConfiguration> configureAction)
        {   
            builder.AddEndpointsInvoker();
            
            var config = new EndpointConfiguration();
            configureAction(config);
            builder.Add<EndpointConfiguration>(config);
            
            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints.
        /// </summary>
        public static IServicesBuilder AddEndpoints(this IServicesBuilder builder, Func<IServiceProvider, EndpointConfiguration> factoryFunction)
        {
            builder.AddEndpointsInvoker();
            
            builder.Add<EndpointConfiguration>(factoryFunction);
            
            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints with the default settings.
        /// </summary>
        public static IServicesBuilder AddEndpoints(this IServicesBuilder builder)
        {
            builder.AddEndpoints(new EndpointConfiguration());
            
            return builder;
        }
    }
}