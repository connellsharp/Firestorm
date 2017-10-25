using System;
using Firestorm.Endpoints;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Adapter that wraps an <see cref="IRestEndpointContext"/> and implements <see cref="IRootRequest"/>.
    /// </summary>
    public class EndpointRootRequest : IRootRequest
    {
        private readonly IRestEndpointContext _endpointContext;

        public EndpointRootRequest(IRestEndpointContext endpointContext)
        {
            _endpointContext = endpointContext;
            endpointContext.OnDispose += (sender, args) => OnDispose?.Invoke(this, args);
        }

        /// <summary>
        /// The logged in user calling the endpoint.
        /// </summary>
        public IRestUser User => _endpointContext.User;

        /// <summary>
        /// Event that is called when the endpoint is disposed.
        /// Can/should be used to attach handlers that dispose of dependencies too e.g. Stems.
        /// </summary>
        public event EventHandler OnDispose;
    }
}