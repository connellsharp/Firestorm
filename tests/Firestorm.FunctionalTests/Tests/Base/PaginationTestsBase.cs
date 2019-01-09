using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.FunctionalTests.Setup;
using Firestorm.Testing.Http;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.FunctionalTests.Tests
{
    public abstract class PaginationTestsBase
    {
        private readonly HttpClient _client;

        protected PaginationTestsBase(IFunctionalTestFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task PlayersCollection_PostMulti_MultiResponseAllSuccess()
        {
            HttpResponseMessage response = await _client.PostAsync("/players", new StringContent(@"[
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
            HttpResponseMessage response = await _client.GetAsync("/players?size=1&fields=name");
            
            ResponseAssert.Success(response);

            IEnumerable<string> linkHeaders = response.Headers.GetValues("Link");
            Assert.Single(linkHeaders);

            string linkHeaderValue = response.Headers.GetValues("Link").Single();
            Assert.Contains("next", linkHeaderValue);
        }
    }
}