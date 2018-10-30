using System.Threading.Tasks;
using Firestorm.AspNetCore2.HttpContext;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2
{
    [UsedImplicitly]
    public class FirestormMiddleware
    {
        private readonly IRequestInvoker _invoker;
        private readonly RequestDelegate _next;

        public FirestormMiddleware(RequestDelegate next, IRequestInvoker invoker)
        {
            _next = next;
            _invoker = invoker;

            // middlewares are singletons constructed during WebHostBuilder.Build(), we can initialize here
            invoker.Initialize();
        }

        [UsedImplicitly]
        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var reader = new HttpContextReader(httpContext);
            var responder = new HttpContextResponder(httpContext);
            var context = new HttpContextRequestContext(httpContext);

            await _invoker.InvokeAsync(reader, responder, context);

            //await _next.Invoke(httpContext);
        }
    }
}