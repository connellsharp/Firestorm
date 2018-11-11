using Firestorm.Host;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2
{
    public static class FirestormServicesExtensions
    {
        /// <summary>
        /// Adds Firestorm services.
        /// </summary>
        public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services)
        {
            IFirestormServicesBuilder builder = new AspNetCoreServicesBuilder(services);
            return builder;
        }
    }
}