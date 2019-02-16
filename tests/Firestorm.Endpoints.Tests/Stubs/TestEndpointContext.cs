using Firestorm.Endpoints.Configuration;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Tests.Stubs
{
    public class TestEndpointContext : IEndpointContext
    {   
        public IEndpointCoreServices Services { get; } = new TestEndpointServices();

        public IRequestContext Request { get; } = new TestRequestContext();
    }
}