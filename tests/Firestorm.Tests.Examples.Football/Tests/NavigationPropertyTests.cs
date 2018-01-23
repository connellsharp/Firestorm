using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
                { vs_team: { id: 2 }, home: false, goals: [ ] },
                { vs_team: { id: 3 }, home: true , goals: [ ] },
            ]"));

            AssertSuccess(response);
        }

        private static void AssertSuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string errorJson = response.Content.ReadAsStringAsync().Result;
                var errorObj = JsonConvert.DeserializeObject<ErrorModel>(errorJson);
                throw new RestApiException((ErrorStatus) response.StatusCode, errorObj.Error + ": " + errorObj.ErrorDescription);
            }
        }

        [Theory, ClassData(typeof(FootballHttpClientIndexes))]
        public async Task CalculateLeaguePosition_Success(FirestormApiTech tech)
        {
            HttpClient client = _fixture.GetClient(tech);

            HttpResponseMessage response = await client.GetAsync("/leagues/premierleague/teams/by_position/1/name");
            response.EnsureSuccessStatusCode();
        }
    }

    internal class ErrorModel
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}