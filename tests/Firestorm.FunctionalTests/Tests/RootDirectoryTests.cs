using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.FunctionalTests.Tests.Setup;
using Xunit;

namespace Firestorm.FunctionalTests.Tests
{
    public class RootDirectoryTests : IClassFixture<FootballTestFixture>
    {
        private readonly FootballTestFixture _fixture;

        public RootDirectoryTests(FootballTestFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Theory, ClassData(typeof(FootballHttpClientIndexes))]
        public async Task Root_DoesntError(FirestormApiTech tech)
        {
            HttpClient client = _fixture.GetClient(tech);

            HttpResponseMessage response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
    }
}