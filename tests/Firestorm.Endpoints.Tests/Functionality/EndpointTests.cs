using System.Linq;
using System.Threading.Tasks;
using Firestorm.Rest.Web;
using Firestorm.Endpoints;
using Firestorm.Host;
using Firestorm.Endpoints.Tests.Stubs;
using Firestorm.Testing;
using Xunit;

namespace Firestorm.Endpoints.Tests.Functionality
{
    public class EndpointTests
    {   
        [Fact]
        public async Task MemoryCollection()
        {
            var endpointContext = new TestEndpointContext();
            IRestEndpoint endpoint =  endpointContext.Configuration.EndpointResolver.GetFromResource(endpointContext, new ArtistMemoryCollection());
            var response = (CollectionBody)(await endpoint.GetAsync(null));
            var firstObj = response.Items.First();
            Assert.Equal(firstObj["Name"], TestRepositories.ArtistName);
        }

        [Fact]
        public void DisposeHttpEndpointContextDisposesParent()
        {
            bool disposed = false;
            var requestContext = new TestRequestContext(); // TODO: kinda just tests the test...
            requestContext.OnDispose += delegate { disposed = true; };
            requestContext.Dispose();
            Assert.True(disposed);
        }
    }
}
