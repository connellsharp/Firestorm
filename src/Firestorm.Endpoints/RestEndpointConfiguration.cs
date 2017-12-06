using System.Collections;
using System.Collections.Generic;
using Firestorm.Endpoints.Pagination;
using Firestorm.Endpoints.Query;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Strategies;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// The configuration required to setup a Firestorm REST API server.
    /// Desribes how to interact with the resources in this API.
    /// </summary>
    public class RestEndpointConfiguration
    {
        /// <summary>
        /// Set to true to enable details exception messages in the error responses.
        /// This should not be used in production.
        /// </summary>
        public bool ShowDeveloperErrors { get; set; } = false;

        /// <summary>
        /// Gets the objects to return in the response body from the return values from <see cref="IRestEndpoint"/> implementations.
        /// </summary>
        public IResponseContentGenerator ResponseContentGenerator { get; set; } = new DirectResponseContentGenerator();

        /// <summary>
        /// The configuration used to build the <see cref="QueryStringCollectionQuery"/> from a requested query string.
        /// </summary>
        public CollectionQueryStringConfiguration QueryStringConfiguration { get; set; } = new CollectionQueryStringConfiguration();

        /// <summary>
        /// Contains 3 sets of strategies (for collection, items and scalars) defining how endpoints behave to unsafe requests.
        /// </summary>
        public UnsafeRequestStrategySets RequestStrategies { get; set; } = new UnsafeRequestStrategySets();

        /// <summary>
        /// The system-wide configuration defining how pagination can work.
        /// </summary>
        public PageConfiguration PageConfiguration { get; set; } = new PageConfiguration();
    }
}