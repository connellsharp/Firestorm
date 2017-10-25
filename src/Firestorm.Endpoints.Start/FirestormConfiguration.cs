using Firestorm.Endpoints.Start;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// The configuration required to setup a Firestorm REST API server.
    /// </summary>
    public class FirestormConfiguration
    {
        /// <summary>
        /// Defines how to get the start <see cref="IRestResource"/> i.e. the root directory.
        /// </summary>
        public IStartResourceFactory StartResourceFactory { get; set; }

        /// <summary>
        /// The configuration discribing how to interact with the resources in this API.
        /// </summary>
        public RestEndpointConfiguration EndpointConfiguration { get; set; } = new RestEndpointConfiguration();
    }
}