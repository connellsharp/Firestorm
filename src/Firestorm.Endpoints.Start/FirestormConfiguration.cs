namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// The services and options required to setup a Firestorm REST API server.
    /// Only one object should be created in the application.
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