using System;

namespace Firestorm.Host
{
    public static class StartResourceFactoryServicesExtensions
    {
        /// <summary>
        /// Configures the start resource for this Firestorm API.
        /// </summary>
        public static IFirestormServicesBuilder AddStartResourceFactory(this IFirestormServicesBuilder builder, IStartResourceFactory startResourceFactory)
        {
            builder.Add<IStartResourceFactory>(startResourceFactory);
            return builder;
        }

        /// <summary>
        /// Configures the start resource for this Firestorm API.
        /// </summary>
        public static IFirestormServicesBuilder AddStartResourceFactory(this IFirestormServicesBuilder builder, Func<IFirestormServiceProvider,IStartResourceFactory> startResourceFactoryFunc)
        {
            builder.Add<IStartResourceFactory>(startResourceFactoryFunc);
            return builder;
        }
    }
}