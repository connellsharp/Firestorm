using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.FunctionalTests.Tests.Setup;
using Firestorm.Testing.Http;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.FunctionalTests.Tests
{
    public class NavigationPropertyTests : IClassFixture<FootballTestFixture>
    {
        private readonly FootballTestFixture _fixture;

        public NavigationPropertyTests(FootballTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory, ClassData(typeof(FootballHttpClientIndexes))]
        public async Task AddFixtures_Success(FirestormApiTech tech)
        {
            HttpClient client = _fixture.GetClient(tech);

            HttpResponseMessage response = await client.PostAsync("/teams/1/fixtures", new StringContent(@"[
                { vs_team: { id: 2 }, home: true, goals: [ ] }
            ]"));

            ResponseAssert.Success(response);
        }

        [Theory, ClassData(typeof(FootballHttpClientIndexes))]
        public async Task CalculateLeaguePoints_Success(FirestormApiTech tech)
        {
            HttpClient client = _fixture.GetClient(tech);
            
            HttpResponseMessage response = await client.GetAsync("/leagues/premierleague/teams?sort=points+desc&limit=1");

            ResponseAssert.Success(response);

            string str = await response.Content.ReadAsStringAsync();
            var arr = JsonConvert.DeserializeObject<object[]>(str);

            Assert.Single(arr);
        }

        [Theory(Skip = "Pretty complex field would want ROW_NUMBER. Perhaps a special 'index' field with order by could be included in future."), ClassData(typeof(FootballHttpClientIndexes))]
        public async Task CalculateLeaguePosition_Success(FirestormApiTech tech)
        {
            HttpClient client = _fixture.GetClient(tech);

            HttpResponseMessage response = await client.GetAsync("/leagues/premierleague/teams/by_position/1/name");

            ResponseAssert.Success(response);
        }
    }
}