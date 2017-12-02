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
                case "artistcore":
                    return IntegratedRestDirectory.GetArtistCollection(_endpointContext);

                case "artists":
                    return _selfRestClient.RequestCollection("artistcore");

                default:
                    return null;
            }
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}