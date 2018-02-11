using Firestorm.Endpoints.Query;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Strategies;
using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// The configuration required to setup a Firestorm REST API server.
    /// Desribes how to interact with the resources in this API.
    /// </summary>
    public class RestEndpointConfiguration
    {
        /// <summary>
        /// The configuration used to build the <see cref="QueryStringCollectionQuery"/> from a requested query string.
        /// </summary>
        public CollectionQueryStringConfiguration QueryStringConfiguration { get; set; } = new CollectionQueryStringConfiguration();

        /// <summary>
        /// Contains 3 sets of strategies (for collection, items and scalars) defining how endpoints behave to unsafe requests.
        /// </summary>
        public UnsafeRequestStrategySets RequestStrategies { get; set; } = new UnsafeRequestStrategySets();

        /// <summary>
        /// The options used to build the responses to return to the client.
        /// </summary>
        public ResponseConfiguruation ResponseConfiguration { get; set; } = new ResponseConfiguruation();
        
        /// <summary>
        /// The object used to convert from .NET Stem member naming conventions to client-side API conventions.
        /// </summary>
        [CanBeNull]
        NamingConventionSwitcher NamingConventionSwitcher { get; }
    }
}