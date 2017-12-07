using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Firestorm.Endpoints.Owin
{
    [UsedImplicitly]
    public class FirestormMiddleware : OwinMiddleware
    {
        private readonly FirestormConfiguration _configuration;

        public FirestormMiddleware(OwinMiddleware next, FirestormConfiguration configuration)
            : base(next)
        {
            _configuration = configuration;

            _configuration.StartResourceFactory.Initialize();
        }

        public override async Task Invoke(IOwinContext owinContext)
        {
            IHttpRequestHandler requestHandler = new OwinContextHandler(owinContext);
            var middlewareHelper = new FirestormMiddlewareHelper(_configuration, requestHandler);

            var restContext = new OwinRestEndpointContext(owinContext, _configuration);

            await middlewareHelper.InvokeAsync(restContext);

            //await Next.Invoke(owinContext);
        }
    }
}