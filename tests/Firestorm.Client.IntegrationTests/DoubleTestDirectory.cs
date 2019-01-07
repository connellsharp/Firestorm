using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;
using Firestorm.Testing.Http;

namespace Firestorm.Client.IntegrationTests
{
    public class DoubleTestDirectory : IRestDirectory
    {
        private readonly IRequestContext _requestContext;
        private readonly RestClient _selfRestClient;

        public DoubleTestDirectory(IRequestContext requestContext, RestClient selfRestClient)
        {
            _requestContext = requestContext;
            _selfRestClient = selfRestClient;
        }

        public IRestResource GetChild(string startResourceName)
        {
            switch (startResourceName)
            {
                case "ArtistCore":
                    return IntegratedRestDirectory.GetArtistCollection(_requestContext);

                case "Artists":
                    return _selfRestClient.RequestCollection("ArtistCore");

                default:
                    return null;
            }
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            return Task.FromResult(new RestDirectoryInfo(new List<RestResourceInfo>
            {
                new RestResourceInfo("Artists", ResourceType.Collection)
            }));
        }
    }
}