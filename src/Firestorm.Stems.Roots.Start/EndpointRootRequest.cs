using System;
using Firestorm.Endpoints;
using Firestorm.Host;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Adapter that wraps an <see cref="IRestEndpointContext"/> and implements <see cref="IRootRequest"/>.
    /// </summary>
    [Obsolete()]
    public class EndpointRootRequest : IRootRequest
    {
        private readonly IRequestContext _requestContext;

        public EndpointRootRequest(IRequestContext requestContext)
        {
            _requestContext = requestContext;
            requestContext.OnDispose += (sender, args) => OnDispose?.Invoke(this, args);
        }

        /// <summary>
        /// The logged in user calling the endpoint.
        /// </summary>
        public IRestUser User => _requestContext.User;

        /// <summary>
        /// Event that is called when the endpoint is disposed.
        /// Can/should be used to attach handlers that dispose of dependencies too e.g. Stems.
        /// </summary>
        public event EventHandler OnDispose;
    }
}