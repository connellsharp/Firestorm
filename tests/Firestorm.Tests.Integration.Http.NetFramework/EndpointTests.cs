using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints;
using Firestorm.Engine;
using Firestorm.Tests.Endpoints.Models;
using Firestorm.Tests.Integration.Http.Base;
using Firestorm.Tests.Models;
using Xunit;

namespace Firestorm.Tests.HttpWebStacks
{
    /// <summary>
    /// Some basic tests for the Endpoint chains using the <see cref="IntegratedRestDirectory"/>.
    /// </summary>
    public class EndpointTests
    {
        [Fact]
        public async Task FieldSelectorManualNext()
        {
            IRestEndpointContext endpointContext = new TestEndpointContext
            {
                SelectFields = new[] { "name" }
            };

            EngineRestCollection<Artist> artistsCollection = IntegratedRestDirectory.GetArtistCollection(endpointContext);
            IRestEndpoint endpoint = Endpoint.GetFromResource(endpointContext, artistsCollection);
            endpoint = endpoint.Next("123");
            var response = (ItemBody)(await endpoint.GetAsync());
            Assert.Equal(response.Item["name"], TestRepositories.ArtistName);
        }
    }
}
