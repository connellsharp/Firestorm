using System;

namespace Firestorm.Host.Infrastructure
{
    public interface IRequestContext : IDisposable
    {
        /// <summary>
        /// The logged in user calling the endpoint.
        /// </summary>
        IRestUser User { get; }

        /// <summary>
        /// Event that is called when the endpoint is disposed.
        /// Can/should be used to attach handlers that dispose of dependencies too e.g. Stems.
        /// </summary>
        event EventHandler OnDispose;
    }
}