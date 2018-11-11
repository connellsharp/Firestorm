using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    [UsedImplicitly]
    public class FirestormMiddleware : OwinMiddleware
    {
        private readonly IRequestInvoker _invoker;

        public FirestormMiddleware(OwinMiddleware next, IRequestInvoker invoker)
            : base(next)
        {
            _invoker = invoker;

            _invoker.Initialize(); // TODO check lifetime of this
        }

        public override async Task Invoke(IOwinContext owinContext)
        {
            IHttpRequestHandler requestHandler = new OwinContextHandler(owinContext);
            
            var reader = new HttpContextReader(httpContext);
            var responder = new HttpContextResponder(httpContext);
            var context = new HttpContextRequestContext(httpContext);
            
            var middlewareHelper = new FirestormMiddlewareHelper(_configuration, requestHandler);

            var restContext = new OwinRestEndpointContext(owinContext, _configuration.EndpointConfiguration);

            await middlewareHelper.InvokeAsync(restContext);
            _invoker.InvokeAsync(restContextm )

            //await Next.Invoke(owinContext);
        }
    }
}