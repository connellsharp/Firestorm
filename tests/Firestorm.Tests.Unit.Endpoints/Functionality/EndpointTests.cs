using System.Linq;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Firestorm.Tests.Unit.Endpoints.Stubs;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Functionality
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
            var navigator = new EndpointNavigator(EndpointContext.Request, StartResourceFactory, EndpointContext.Configuration);
            IRestEndpoint endpoint = navigator.GetEndpointFromPath("artists");
            
            var response = (CollectionBody)(await endpoint.GetAsync(null));

            RestItemData[] arr = response.Items.ToArray();
            int firstId = (int)arr[0]["ID"];
            Assert.True(firstId > 0);
        }

        [Fact]
        public async Task ItemEndpoint()
        {
            var navigator = new EndpointNavigator(EndpointContext.Request, StartResourceFactory, EndpointContext.Configuration);
            IRestEndpoint endpoint = navigator.GetEndpointFromPath("artists/123");
            
            ItemBody itemBody = (ItemBody)(await endpoint.GetAsync(null));

            RestItemData itemData = itemBody.Item;
            Assert.Equal("ID", itemData.Keys.First());

            int firstId = (int)itemData["ID"];
            Assert.True(firstId > 0);
        }

        [Fact]
        public async Task DictionaryEndpoint()
        {
            var navigator = new EndpointNavigator(EndpointContext.Request, StartResourceFactory, EndpointContext.Configuration);
            IRestEndpoint endpoint = navigator.GetEndpointFromPath("artists/by_ID");
            
            DictionaryBody dictionaryBody = (DictionaryBody)(await endpoint.GetAsync(null));

            foreach (var pair in dictionaryBody.Items)
            {
                var itemData = pair.Value as RestItemData;
                int id = (int)itemData["ID"];

                Assert.True(id > 0);
                Assert.Equal(pair.Key, id.ToString());
            }
        }

        [Fact]
        public async Task FieldEndpoint()
        {
            var navigator = new EndpointNavigator(EndpointContext.Request, StartResourceFactory, EndpointContext.Configuration);
            IRestEndpoint endpoint = navigator.GetEndpointFromPath("artists/123/Name");


            ResourceBody response = await endpoint.GetAsync(null);
            Assert.Equal(response.GetObject(), TestRepositories.ArtistName);
        }

        [Fact]
        public async Task MemoryCollection()
        {
            IRestEndpoint endpoint = Endpoint.GetFromResource(EndpointContext, new ArtistMemoryCollection());
            var response = (CollectionBody)(await endpoint.GetAsync(null));
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
