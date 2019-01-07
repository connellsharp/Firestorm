using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Firestorm.Tests.Examples.Football.Tests
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