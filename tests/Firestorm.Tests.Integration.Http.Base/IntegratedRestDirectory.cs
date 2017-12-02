using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Core;
using Firestorm.Endpoints;
using Firestorm.Engine;
using Firestorm.Tests.Integration.Http.Base.Models;
using Firestorm.Tests.Models;

namespace Firestorm.Tests.Integration.Http.Base
{
    public class IntegratedRestDirectory : IRestDirectory
    {
        private readonly IRestEndpointContext _endpointContext;

        public IntegratedRestDirectory(IRestEndpointContext endpointContext)
        {
            _endpointContext = endpointContext;
        }

        public IRestResource GetChild(string startResourceName)
        {
            switch (startResourceName)
            {
                case "artists":
                    return GetArtistCollection(_endpointContext);

                default:
                    return null;
            }
        }

        public static EngineRestCollection<Artist> GetArtistCollection(IRestEndpointContext endpointContext)
        {
            return new EngineRestCollection<Artist>(new CodedArtistEntityContext(endpointContext.User));
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            return Task.FromResult(new RestDirectoryInfo(new List<RestResourceInfo>
            {
                new RestResourceInfo("artists", ResourceType.Collection)
            }));
        }
    }
}