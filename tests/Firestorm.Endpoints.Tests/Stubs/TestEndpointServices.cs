using Firestorm.Endpoints.Configuration;

namespace Firestorm.Endpoints.Tests.Stubs
{
    public class TestEndpointServices : EndpointServices
    {
        public TestEndpointServices()
        {
            var config = new EndpointConfiguration();
            config.NamingConventions.TwoLetterAcronyms = new[] {"ID"};
            new DefaultEndpointFeature(config).AddTo(this);
        }
    }
}