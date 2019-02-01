using Firestorm.Endpoints.Configuration;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    internal class EndpointContext : IEndpointContext
    {
        public EndpointContext(IRequestContext endpointContext, IEndpointServices services)
        {
            Request = endpointContext;
            Services = services;
        }

        public IEndpointServices Services { get; }

        public IRequestContext Request { get; }

        public void Dispose()
        {
            Request.Dispose();
        }
    }
}