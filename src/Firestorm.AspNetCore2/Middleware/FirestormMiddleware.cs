using System.Threading.Tasks;
using Firestorm.AspNetCore2.HttpContext;
using Firestorm.Endpoints.Start;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Firestorm.AspNetCore2
{
    [UsedImplicitly]
    public class FirestormMiddleware
    {
        private readonly FirestormConfiguration _configuration;
        private readonly RequestDelegate _next;

        public FirestormMiddleware(RequestDelegate next, FirestormConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;

            _configuration.StartResourceFactory.Initialize();
        }

        [UsedImplicitly]
        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            IHttpRequestHandler requestHandler = new HttpContextHandler(httpContext);
            var middlewareHelper = new FirestormMiddlewareHelper(_configuration, requestHandler);

            var restContext = new HttpContextRestEndpointContext(httpContext, _configuration.EndpointConfiguration);

            await middlewareHelper.InvokeAsync(restContext);

            //await _next.Invoke(httpContext);
        }
    }
}