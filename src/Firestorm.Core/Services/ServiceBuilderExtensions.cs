using System;
using JetBrains.Annotations;

namespace Firestorm
{
    [PublicAPI]
    public static class ServiceBuilderExtensions
    {
        public static IFirestormServicesBuilder Add<TAbstraction, TImplementation>(this IFirestormServicesBuilder builder)
            where TImplementation : class, TAbstraction
            where TAbstraction : class
        {
            return builder.Add(typeof(TAbstraction), typeof(TImplementation));
        }
        
        public static IFirestormServicesBuilder Add<TService>(this IFirestormServicesBuilder builder)
            where TService : class
        {
            return builder.Add(typeof(TService), typeof(TService));
        }
        
        public static IFirestormServicesBuilder Add(this IFirestormServicesBuilder builder, Type serviceType)
        {
            return builder.Add(serviceType, serviceType);
        }
    }
}