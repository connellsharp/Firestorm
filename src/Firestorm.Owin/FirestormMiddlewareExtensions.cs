using System;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Owin;

namespace Firestorm.Owin
{
    public static class FirestormMiddlewareExtensions
    {
        public static IAppBuilder UseFirestorm(this IAppBuilder app, FirestormConfiguration configuration)
        {
            app.Use<FirestormMiddleware>(configuration);
            return app;
        }
        
        public static IAppBuilder UseFirestorm(this IAppBuilder app, Action<IFirestormServicesBuilder> configureAction)
        {
            var servicesBuilder = new OwinServicesBuilder(app);
            configureAction(servicesBuilder);
            var serviceProvider = servicesBuilder.Build();
            
            app.Use<FirestormMiddleware>(serviceProvider.GetService<IRequestInvoker>());
            return app;
        }
    }

    public class OwinServicesBuilder : IFirestormServicesBuilder
    {
        public IServiceProvider Build()
        {
            throw new NotImplementedException();
        }
    }
}
