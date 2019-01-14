using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.Strategies;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// The default options and services required to setup a Firestorm REST API server.
    /// </summary>
    public class DefaultRestEndpointConfiguration : RestEndpointConfiguration
    {
        public DefaultRestEndpointConfiguration()
        {
            CommandStrategies = new CommandStrategySets();
            NamingConventionSwitcher = new DefaultNamingConventionSwitcher();
            EndpointResolver = new EndpointResolver();
        }
    }
}