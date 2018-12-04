using Firestorm.Endpoints.Naming;
using Firestorm.Endpoints.Strategies;

namespace Firestorm.Endpoints.Web
{
    /// <summary>
    /// The default options and services required to setup a Firestorm REST API server.
    /// </summary>
    public class DefaultRestEndpointConfiguration : RestEndpointConfiguration
    {
        public DefaultRestEndpointConfiguration()
        {
            RequestStrategies = new UnsafeRequestStrategySets();
            NamingConventionSwitcher = new DefaultNamingConventionSwitcher();
            Resolver = new EndpointResolver();
        }
    }
}