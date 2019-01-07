using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Engine;
using Firestorm.Host;
using Firestorm.Testing.Http.Models;
using Firestorm.Testing.Models;
using Firestorm.Testing;

namespace Firestorm.Testing.Http
{
    public class IntegratedRestDirectory : IRestDirectory
    {
        private const string ArtistRootName = nameof(Artist) + "s";

        private readonly IRequestContext _requestContext;

        public IntegratedRestDirectory(IRequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        public IRestResource GetChild(string startResourceName)
        {
            switch (startResourceName)
            {
                case ArtistRootName:
                    return GetArtistCollection(_requestContext);

                default:
                    return null;
            }
        }

        public static EngineRestCollection<Artist> GetArtistCollection(IRequestContext requestContext)
        {
            return new EngineRestCollection<Artist>(new CodedArtistEntityContext(requestContext.User));
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            return Task.FromResult(new RestDirectoryInfo(new List<RestResourceInfo>
            {
                new RestResourceInfo(ArtistRootName, ResourceType.Collection)
            }));
        }
    }
}