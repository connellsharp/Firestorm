using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Firestorm.Tests.Examples.Football.Tests
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
                { teams: [ { id: 2 } ], home: true, goals: [ ] },
                { teams: [ { id: 3, home: true } ], goals: [ ] },
            ]"));

            response.EnsureSuccessStatusCode();
        }

        [Theory, ClassData(typeof(FootballHttpClientIndexes))]
        public async Task CalculateLeaguePosition_Success(FirestormApiTech tech)
        {
            HttpClient client = _fixture.GetClient(tech);

            HttpResponseMessage response = await client.GetAsync("/leagues/premierleague/teams/by_position/1/name");
            response.EnsureSuccessStatusCode();
        }
    }
}