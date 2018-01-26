using Firestorm.Endpoints.Start;
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
    }
}
