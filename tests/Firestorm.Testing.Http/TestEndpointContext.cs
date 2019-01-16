using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Testing.Http
{
    public class TestEndpointContext : IEndpointContext
    {
        public EndpointConfiguration Configuration { get; } = new DefaultEndpointConfiguration();
        
        public IRequestContext Request { get; } = new TestRequestContext();
    }
}