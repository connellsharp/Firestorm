using System.Collections.Generic;
using Firestorm.Endpoints.Requests;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Configuration
{
    /// <summary>
    /// The services required to setup a Firestorm REST API server.
    /// </summary>
    public interface IEndpointServices
    {
        /// <summary>
        /// Calculates the URLs to the next and previous pages when querying a collection.
        /// </summary>
        IPageLinkCalculator PageLinkCalculator { get; }

        /// <summary>
        /// Contains 3 sets of strategies (for collection, items and scalars) defining how endpoints behave to unsafe requests.
        /// </summary>
        ICommandStrategySets Strategies { get; }

        /// <summary>
        /// The object used to convert from .NET member naming conventions to client-side API conventions.
        /// </summary>
        [CanBeNull]
        INamingConventionSwitcher NameSwitcher { get; }

        /// <summary>
        /// The object used to resolve <see cref="IRestEndpoint"/> instances from a <see cref="IRestResource"/>.
        /// </summary>
        IEndpointResolver EndpointResolver { get; }

        /// <summary>
        /// Interprets a URL path.
        /// </summary>
        IUrlHelper UrlHelper { get; }
    }
}