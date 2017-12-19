using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Examples.Football.Tests
{
    public class PaginationTests : IClassFixture<FootballTestFixture>
    {
        private readonly FootballTestFixture _fixture;

        public PaginationTests(FootballTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task PlayersCollection_PostMulti_MultiResponseAllSuccess()
        {
            HttpResponseMessage response = await _fixture.HttpClient.PostAsync("/players", new StringContent(@"[
                { name: ""Romelu Lukaku"" },
                { name: ""Anthony Martial"" },
                { name: ""Marcus Rashford"" },
                { name: ""Paul Pogba"" },
            ]"));

            Assert.Equal(207, (int)response.StatusCode);

            string jsonStr = await response.Content.ReadAsStringAsync();
            var successObjs = JsonConvert.DeserializeObject<StatusModel[]>(jsonStr);

            Assert.Contains(successObjs, sm =>
            {
                return sm.Status == "created";
            });
        }

        private class StatusModel
        {
            [JsonProperty("status")]
            public string Status { get; set; }
        }

        [Fact]
        public async Task PlayersCollection_PageSize1_NextInLinkHeader()
        {
            HttpResponseMessage response = await _fixture.HttpClient.GetAsync("/players?size=1");

            Assert.Equal(200, (int)response.StatusCode);

            IEnumerable<string> linkHeaders = response.Headers.GetValues("Link");
            Assert.Equal(1, linkHeaders.Count());

            string linkHeaderValue = response.Headers.GetValues("Link").Single();
            Assert.Contains("next", linkHeaderValue);
        }
    }
}
