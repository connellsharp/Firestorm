using Firestorm.Endpoints.Requests;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Configuration
{
    /// <summary>
    /// The options and services required to setup a Firestorm REST API server.
    /// Describes how to interact with the resources in this API.
    /// </summary>
    public class RestEndpointConfiguration
    {
        /// <summary>
        /// The configuration used to build the <see cref="QueryStringCollectionQuery"/> from a requested query string.
        /// </summary>
        public QueryStringConfiguration QueryStringConfiguration { get; set; } = new QueryStringConfiguration();

        /// <summary>
        /// Contains 3 sets of strategies (for collection, items and scalars) defining how endpoints behave to unsafe requests.
        /// </summary>
        public ICommandStrategySets CommandStrategies { get; set; }

        /// <summary>
        /// The options used to build the responses to return to the client.
        /// </summary>
        public ResponseConfiguration ResponseConfiguration { get; set; } = new ResponseConfiguration();
        
        /// <summary>
        /// The object used to convert from .NET member naming conventions to client-side API conventions.
        /// </summary>
        [CanBeNull]
        public INamingConventionSwitcher NamingConventionSwitcher { get; set; }

        /// <summary>
        /// The object used to resolve <see cref="IRestEndpoint"/> instances from a <see cref="IRestResource"/>.
        /// </summary>
        public IEndpointResolver EndpointResolver { get; set; }
    }
}