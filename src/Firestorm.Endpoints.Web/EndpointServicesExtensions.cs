using System;
using Firestorm.Host;

namespace Firestorm.Endpoints.Web
{
    public static class EndpointServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm endpoints.
        /// Assumes <see cref="RestEndpointConfiguration"/> is already configured elsewhere.
        /// </summary>
        public static IFirestormServicesBuilder AddEndpointsInvoker(this IFirestormServicesBuilder builder)
        {   
            builder.Add<IRequestInvoker, EndpointsRequestInvoker>();
            
            return builder;
        }
        
        /// <summary>
        /// Configures Firestorm endpoints.
        /// </summary>
        public static IFirestormServicesBuilder AddEndpoints(this IFirestormServicesBuilder builder, RestEndpointConfiguration config)
        {
            builder.AddEndpointsInvoker();
            
            builder.Add<RestEndpointConfiguration>(config);
            
            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints.
        /// </summary>
        public static IFirestormServicesBuilder AddEndpoints(this IFirestormServicesBuilder builder, Action<RestEndpointConfiguration> configureAction)
        {   
            builder.AddEndpointsInvoker();
            
            var config = new DefaultRestEndpointConfiguration();
            configureAction(config);
            builder.Add<RestEndpointConfiguration>(config);
            
            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints.
        /// </summary>
        public static IFirestormServicesBuilder AddEndpoints(this IFirestormServicesBuilder builder, Func<IServiceProvider, RestEndpointConfiguration> factoryFunction)
        {
            builder.AddEndpointsInvoker();
            
            builder.Add<RestEndpointConfiguration>(factoryFunction);
            
            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints with the default settings.
        /// </summary>
        public static IFirestormServicesBuilder AddEndpoints(this IFirestormServicesBuilder builder)
        {
            builder.AddEndpoints(new DefaultRestEndpointConfiguration());
            
            return builder;
        }
    }
}