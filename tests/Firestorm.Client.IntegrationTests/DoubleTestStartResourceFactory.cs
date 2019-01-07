using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Client.IntegrationTests
{
    /// <summary>
    /// Creates a weird double test where the 'artists' collection is straight from memory data and 'artists2' is an HTTP client that loops back to 'artists'.
    /// </summary>
    public class DoubleTestStartResourceFactory : IStartResourceFactory
    {
        private readonly RestClient _selfRestClient;

        public DoubleTestStartResourceFactory(string selfBaseAddress)
        {
            _selfRestClient = new RestClient(selfBaseAddress);
        }

        public IRestResource GetStartResource(IRequestContext requestContext)
        {
            return new DoubleTestDirectory(requestContext, _selfRestClient);
        }

        public void Initialize()
        {
        }
    }
}