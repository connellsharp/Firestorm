using Firestorm.Endpoints.Configuration;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    internal class EndpointContext : IEndpointContext
    {
        public EndpointContext(IRequestContext endpointContext, EndpointConfiguration configuration)
        {
            Request = endpointContext;
            Configuration = configuration;
        }

        public EndpointConfiguration Configuration { get; }

        public IRequestContext Request { get; }

        public void Dispose()
        {
            Request.Dispose();
        }
    }
}