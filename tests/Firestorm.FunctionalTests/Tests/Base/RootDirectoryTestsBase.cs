using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.FunctionalTests.Setup;
using Xunit;

namespace Firestorm.FunctionalTests.Tests
{
    public abstract class RootDirectoryTestsBase
    {
        private readonly HttpClient _client;

        protected RootDirectoryTestsBase(IFunctionalTestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Root_DoesntError()
        {
            HttpResponseMessage response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
    }
}