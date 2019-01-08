using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Testing.Http
{
    public class TestEndpointContext : IEndpointContext
    {
        public RestEndpointConfiguration Configuration { get; } = new DefaultRestEndpointConfiguration();
        
        public IRequestContext Request { get; } = new TestRequestContext();
    }
}