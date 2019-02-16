using Firestorm.Endpoints.Configuration;

namespace Firestorm.Endpoints.Tests.Stubs
{
    public class TestEndpointServices : EndpointServices
    {
        public TestEndpointServices()
        {
            new DefaultEndpointFeature(new EndpointConfiguration()).AddTo(this);
        }
    }
}