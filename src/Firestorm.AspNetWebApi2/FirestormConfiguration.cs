using Firestorm.Endpoints;
using Firestorm.Host;

namespace Firestorm.AspNetWebApi2
{
    /// <summary>
    /// The services and options required to setup a Firestorm REST API server.
    /// Only one object should be created in the application.
    /// </summary>
    internal class FirestormConfiguration
    {
        /// <summary>
        /// Defines how to get the start <see cref="IRestResource"/> i.e. the root directory.
        /// </summary>
        public IStartResourceFactory StartResourceFactory { get; set; }

        /// <summary>
        /// The services used to interact with the resources in this API.
        /// </summary>
        public EndpointServices EndpointServices { get; set; }
    }
}