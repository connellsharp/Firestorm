using System;
using Firestorm.Endpoints.Start;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Endpoints.AspNetCore
{
    public static class FirestormServicesExtensions
    {
        /// <summary>
        /// Adds Firestorm services.
        /// </summary>
        public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services)
        {
            IFirestormServicesBuilder builder = new FirestormServicesBuilder(services);
            return builder;
        }

        /// <summary>
        /// Adds Firestorm services and configures them using the given action.
        /// </summary>
        public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services, Action<FirestormConfiguration> configureAction)
        {
            return services.AddFirestorm()
                .Configure(configureAction);
        }

        /// <summary>
        /// Configures Firestorm using the given action.
        /// </summary>
        public static IFirestormServicesBuilder Configure(this IFirestormServicesBuilder builder, Action<FirestormConfiguration> configureAction)
        {
            builder.Services.Configure(configureAction);
            return builder;
        }
    }
}
