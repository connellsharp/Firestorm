using System;
using Firestorm.Defaults;

namespace Firestorm
{
    public static class ServiceProviderExtensions
    {
        public static IRequestServiceProvider GetRequestServiceProvider(this IServiceProvider serviceProvider)
        {
            return (IRequestServiceProvider)serviceProvider.GetService(typeof(IRequestServiceProvider));
        }
    }
    
}