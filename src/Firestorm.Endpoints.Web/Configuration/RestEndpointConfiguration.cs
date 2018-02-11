using Firestorm.Endpoints.Naming;
using Firestorm.Endpoints.Query;
using Firestorm.Endpoints.Responses;
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
            QueryStringConfiguration = new QueryStringConfiguration();
            RequestStrategies = new UnsafeRequestStrategySets();
            ResponseConfiguration = new ResponseConfiguration();
            NamingConventionSwitcher = new DefaultNamingConventionSwitcher();
        }
    }
}