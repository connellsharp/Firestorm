using System.Linq;
using System.Threading.Tasks;
using Firestorm.Core;
using Firestorm.Core.Web;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Firestorm.Tests.Endpoints.Models;
using Firestorm.Tests.Models;
using Xunit;

namespace Firestorm.Tests.Endpoints
{
    /// <summary>
    /// Some basic tests for the Endpoint chains using the <see cref="TestRestDirectory"/>.
    /// </summary>
    public class EndpointTests
    {
        public EndpointTests()
        {
            EndpointContext = new TestEndpointContext();
            StartResourceFactory = new SingletonStartResourceFactory(new TestRestDirectory());
        }

        public TestEndpointContext EndpointContext { get; set; }

        public IStartResourceFactory StartResourceFactory { get; set; }

        [Fact]
        public async Task ListArtists()
        {

            IRestEndpoint endpoint = StartUtilities.GetEndpointFromPath(StartResourceFactory, EndpointContext, "artists");
            var response = (CollectionBody)(await endpoint.GetAsync());
            RestItemData[] arr = response.Items.ToArray();
            int firstId = (int)arr[0]["ID"];
            Assert.True(firstId > 0);
        }

        [Fact]
        public async Task ItemEndpoint()
        {
            IRestEndpoint endpoint = StartUtilities.GetEndpointFromPath(StartResourceFactory, EndpointContext, "artists/123");
            ItemBody itemBody = (ItemBody)(await endpoint.GetAsync());

            RestItemData itemData = itemBody.Item;
            int firstId = (int)itemData["ID"];
            Assert.True(firstId > 0);
        }

        [Fact]
        public async Task FieldEndpoint()
        {
            IRestEndpoint endpoint = StartUtilities.GetEndpointFromPath(StartResourceFactory, EndpointContext, "artists/123/Name");
            ResourceBody response = await endpoint.GetAsync();
            Assert.Equal(response.GetObject(), TestRepositories.ArtistName);
        }

        [Fact]
        public async Task MemoryCollection()
        {
            IRestEndpoint endpoint = Endpoint.GetFromResource(EndpointContext, new ArtistMemoryCollection());
            var response = (CollectionBody)(await endpoint.GetAsync());
            var firstObj = response.Items.First();
            Assert.Equal(firstObj["Name"], TestRepositories.ArtistName);
        }

        [Fact]
        public void DisposeHttpEndpointContextDisposesParent()
        {
            bool disposed = false;
            IRestEndpointContext endpointContext = new TestEndpointContext(); // TODO: kinda just tests the test...
            endpointContext.OnDispose += delegate { disposed = true; };
            endpointContext.Dispose();
            Assert.True(disposed);
        }
    }
}
