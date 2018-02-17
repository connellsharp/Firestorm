using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Tests.Integration.Http.Base;
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
        
        [Theory, ClassData(typeof(FootballHttpClientIndexes))]
        public async Task PlayersCollection_PostMulti_MultiResponseAllSuccess(FirestormApiTech tech)
        {
            HttpClient client = _fixture.GetClient(tech);

            HttpResponseMessage response = await client.PostAsync("/players", new StringContent(@"[
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

        [Theory, ClassData(typeof(FootballHttpClientIndexes))]
        public async Task PlayersCollection_PageSize1_NextInLinkHeader(FirestormApiTech tech)
        {
            HttpClient client = _fixture.GetClient(tech);

            HttpResponseMessage response = await client.GetAsync("/players?size=1&fields=name");
            
            ResponseAssert.Success(response);

            IEnumerable<string> linkHeaders = response.Headers.GetValues("Link");
            Assert.Equal(1, linkHeaders.Count());

            string linkHeaderValue = response.Headers.GetValues("Link").Single();
            Assert.Contains("next", linkHeaderValue);
        }
    }
}
