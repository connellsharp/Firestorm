using Firestorm.Endpoints.Configuration;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    internal class EndpointContext : IEndpointContext
    {
        public EndpointContext(IRequestContext endpointContext, IEndpointCoreServices services)
        {
            Request = endpointContext;
            Services = services;
        }

        public IEndpointCoreServices Services { get; }

        public IRequestContext Request { get; }
    }
}