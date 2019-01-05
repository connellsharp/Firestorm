using System;
using Firestorm.Host;
using Owin;

namespace Firestorm.Owin
{
    public static class FirestormMiddlewareExtensions
    {   
        public static IAppBuilder UseFirestorm(this IAppBuilder app, Action<IFirestormServicesBuilder> configureAction)
        {
            var servicesBuilder = new DefaultServicesBuilder();
            configureAction(servicesBuilder);
            var serviceProvider = servicesBuilder.Build();
            
            app.Use<FirestormMiddleware>(serviceProvider.GetService<IRequestInvoker>());
            return app;
        }
    }
}
