using System;

namespace Firestorm
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <remarks>
        /// Duplicate of extension method in Microsoft.Extensions.DependencyInjection.
        /// </remarks>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service object from.</param>
        /// <returns>A service object of type <typeparamref name="T"/> or null if there is no such service.</returns>
        public static T GetService<T>(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            return (T) provider.GetService(typeof(T));
        }

        public static IRequestServiceProvider GetRequestServiceProvider(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<IRequestServiceProvider>() ??
                   throw new NotImplementedException("Request service provider is not implemented.");
        }
    }
}