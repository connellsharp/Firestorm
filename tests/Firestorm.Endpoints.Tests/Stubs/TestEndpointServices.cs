using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Formatting.Naming;

namespace Firestorm.Endpoints.Tests.Stubs
{
    public class TestEndpointServices : EndpointServices
    {
        public TestEndpointServices()
        {
            var config = new EndpointConfiguration();
            //config.NamingConventions.TwoLetterAcronyms = new[] {"ID"};
            new DefaultEndpointFeature(config).AddTo(this);

            NameSwitcher = new VoidNamingConventionSwitcher();
        }
    }
}