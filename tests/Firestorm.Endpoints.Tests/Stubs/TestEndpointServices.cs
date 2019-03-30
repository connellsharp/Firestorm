using Firestorm.Endpoints.Formatting.Naming;

namespace Firestorm.Endpoints.Tests.Stubs
{
    public class TestEndpointServices : EndpointServices
    {
        public TestEndpointServices()
        {
            var config = new EndpointConfiguration();
            //config.NamingConventions.TwoLetterAcronyms = new[] {"ID"};
            new DefaultEndpointCustomization(config).Apply(this);

            NameSwitcher = new VoidNamingConventionSwitcher();
        }
    }
}