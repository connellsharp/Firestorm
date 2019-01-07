using Firestorm.Endpoints.Configuration;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Web
{
    internal class EndpointContext : IEndpointContext
    {
        public EndpointContext(IRequestContext endpointContext, RestEndpointConfiguration configuration)
        {
            Request = endpointContext;
            Configuration = configuration;
        }

        public RestEndpointConfiguration Configuration { get; }

        public IRequestContext Request { get; }

        public void Dispose()
        {
            Request.Dispose();
        }
    }
}