using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.FunctionalTests.Setup;
using Firestorm.Testing.Http;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.FunctionalTests.Tests
{
    public abstract class NavigationPropertyTestsBase
    {
        private readonly HttpClient _client;

        protected NavigationPropertyTestsBase(IFunctionalTestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task AddFixtures_Success()
        {
            HttpResponseMessage response = await _client.PostAsync("/teams/1/fixtures", new StringContent(@"[
                { vs_team: { id: 2 }, home: true, goals: [ ] }
            ]"));

            ResponseAssert.Success(response);
        }

        [Fact]
        public async Task CalculateLeaguePoints_Success()
        {            
            HttpResponseMessage response = await _client.GetAsync("/leagues/premierleague/teams?sort=points+desc&limit=1");

            ResponseAssert.Success(response);

            string str = await response.Content.ReadAsStringAsync();
            var arr = JsonConvert.DeserializeObject<object[]>(str);

            Assert.Single(arr);
        }

        [Fact(Skip = "Pretty complex field would want ROW_NUMBER. Perhaps a special 'index' field with order by could be included in future.")]
        public async Task CalculateLeaguePosition_Success()
        {
            HttpResponseMessage response = await _client.GetAsync("/leagues/premierleague/teams/by_position/1/name");

            ResponseAssert.Success(response);
        }
    }
}