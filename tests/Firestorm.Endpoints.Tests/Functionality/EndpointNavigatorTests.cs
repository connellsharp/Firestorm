using System.Linq;
using System.Threading.Tasks;
using Firestorm.Rest.Web;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Tests.Stubs;
using Firestorm.Testing;
using Xunit;

namespace Firestorm.Endpoints.Tests.Functionality
{
    /// <summary>
    /// Some basic tests for the Endpoint chains using the <see cref="TestRestDirectory"/>.
    /// </summary>
    public class EndpointNavigatorTests
    {
        private readonly EndpointNavigator _navigator;

        public EndpointNavigatorTests()
        {
            var startResourceFactory = new SingletonStartResourceFactory(new TestRestDirectory());

            var config = new RestEndpointConfiguration
            {
                EndpointResolver = new EndpointResolver()
            };
            
            _navigator = new EndpointNavigator(new TestRequestContext(), startResourceFactory, config);
        }

        [Fact]
        public async Task ListArtists()
        {
            IRestEndpoint endpoint = _navigator.GetEndpointFromPath("artists");

            var response = (CollectionBody) (await endpoint.GetAsync(null));

            RestItemData[] arr = response.Items.ToArray();
            int firstId = (int) arr[0]["ID"];
            Assert.True(firstId > 0);
        }

        [Fact]
        public async Task ItemEndpoint()
        {
            IRestEndpoint endpoint = _navigator.GetEndpointFromPath("artists/123");

            ItemBody itemBody = (ItemBody) (await endpoint.GetAsync(null));

            RestItemData itemData = itemBody.Item;
            Assert.Equal("ID", itemData.Keys.First());

            int firstId = (int) itemData["ID"];
            Assert.True(firstId > 0);
        }

        [Fact]
        public async Task DictionaryEndpoint()
        {
            IRestEndpoint endpoint = _navigator.GetEndpointFromPath("artists/by_ID");

            DictionaryBody dictionaryBody = (DictionaryBody) (await endpoint.GetAsync(null));

            foreach (var pair in dictionaryBody.Items)
            {
                var itemData = pair.Value as RestItemData;
                int id = (int) itemData["ID"];

                Assert.True(id > 0);
                Assert.Equal(pair.Key, id.ToString());
            }
        }

        [Fact]
        public async Task FieldEndpoint()
        {
            IRestEndpoint endpoint = _navigator.GetEndpointFromPath("artists/123/Name");

            ResourceBody response = await endpoint.GetAsync(null);
            Assert.Equal(response.GetObject(), TestRepositories.ArtistName);
        }
    }
}