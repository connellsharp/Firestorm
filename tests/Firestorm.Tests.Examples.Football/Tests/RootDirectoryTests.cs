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

        [Fact]
        public async Task Root_DoesntError()
        {
            HttpResponseMessage response = await _fixture.HttpClient.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
    }
}