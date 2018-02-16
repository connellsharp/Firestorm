using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Client;
using Firestorm.Endpoints;
using Firestorm.Tests.Integration.Http.Base;

namespace Firestorm.Tests.Integration.Http.NetFramework.Web
{
    public class DoubleTestDirectory : IRestDirectory
    {
        private readonly IRestEndpointContext _endpointContext;
        private readonly RestClient _selfRestClient;

        public DoubleTestDirectory(IRestEndpointContext endpointContext, RestClient selfRestClient)
        {
            _endpointContext = endpointContext;
            _selfRestClient = selfRestClient;
        }

        public IRestResource GetChild(string startResourceName)
        {
            switch (startResourceName)
            {
                case "ArtistCore":
                    return IntegratedRestDirectory.GetArtistCollection(_endpointContext);

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