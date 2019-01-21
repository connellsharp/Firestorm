using System;
using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;
using Firestorm.Testing.Http;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Firestorm.Stems.FunctionalTests.Web
{
    /// <summary>
    /// Middleware for when the Firestorm middleware can't even handle the errors.
    /// We still use the format that matches the <see cref="DeveloperExceptionInfoResponseModifier"/>.
    /// Note: naming conventions are not taken into account here.
    /// </summary>
    public class CriticalJsonErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public CriticalJsonErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        [UsedImplicitly]
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                string json = ExceptionUtility.GetExceptionJson(ex);
                await context.Response.WriteAsync(json);
            }
        }
    }
}