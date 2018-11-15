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
        public static IFirestormServicesBuilder AddEndpoints(this IFirestormServicesBuilder builder)
        {   
            builder.Add<IRequestInvoker, EndpointsRequestInvoker>();
            
            return builder;
        }
        
        /// <summary>
        /// Configures Firestorm endpoints.
        /// </summary>
        public static IFirestormServicesBuilder AddEndpoints(this IFirestormServicesBuilder builder, RestEndpointConfiguration config)
        {
            builder.AddEndpoints();
            builder.Add<RestEndpointConfiguration>(config);
            
            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints.
        /// </summary>
        public static IFirestormServicesBuilder AddEndpoints(this IFirestormServicesBuilder builder, Action<RestEndpointConfiguration> configureAction)
        {   
            var config = new DefaultRestEndpointConfiguration();
            configureAction(config);

            builder.AddEndpoints(config);
            
            return builder;
        }

        /// <summary>
        /// Configures Firestorm endpoints with the default settings.
        /// </summary>
        public static IFirestormServicesBuilder AddEndpointsWithDefaultConfig(this IFirestormServicesBuilder builder)
        {
            builder.AddEndpoints(new DefaultRestEndpointConfiguration());
            
            return builder;
        }
    }
}