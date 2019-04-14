using System;
using JetBrains.Annotations;

namespace Firestorm
{
    [PublicAPI]
    public static class ServicesBuilderExtensions
    {
        public static IServicesBuilder Add<TAbstraction, TImplementation>(this IServicesBuilder builder)
            where TImplementation : class, TAbstraction
            where TAbstraction : class
        {
            return builder.Add(typeof(TAbstraction), typeof(TImplementation));
        }
        
        public static IServicesBuilder Add<TService>(this IServicesBuilder builder)
            where TService : class
        {
            return builder.Add(typeof(TService), typeof(TService));
        }
        
        public static IServicesBuilder Add(this IServicesBuilder builder, Type serviceType)
        {
            return builder.Add(serviceType, serviceType);
        }
    }
}