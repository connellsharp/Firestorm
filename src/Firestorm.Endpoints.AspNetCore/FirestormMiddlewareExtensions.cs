using System;
using Firestorm.Endpoints.Start;
using Microsoft.AspNetCore.Builder;

namespace Firestorm.Endpoints.AspNetCore
{
    public static class FirestormMiddlewareExtensions
    {
        public static IApplicationBuilder UseFirestorm(this IApplicationBuilder app, FirestormConfiguration configuration)
        {
            app.UseMiddleware<FirestormMiddleware>(configuration);
            return app;
        }

        public static IApplicationBuilder UseFirestorm(this IApplicationBuilder app, Action<FirestormConfiguration> configure)
        {
            var configuration = new FirestormConfiguration();
            configure(configuration);
            app.UseFirestorm(configuration);
            return app;
        }
    }
}
