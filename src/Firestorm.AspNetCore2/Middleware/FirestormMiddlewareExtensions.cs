using Microsoft.AspNetCore.Builder;

namespace Firestorm.AspNetCore2
{
    public static class FirestormMiddlewareExtensions
    {
        /// <summary>
        /// Configures Firestorm using the services and options configured in the app.
        /// </summary>
        public static IApplicationBuilder UseFirestorm(this IApplicationBuilder app)
        {
            app.UseMiddleware<FirestormMiddleware>();
            return app;
        }
    }
}
