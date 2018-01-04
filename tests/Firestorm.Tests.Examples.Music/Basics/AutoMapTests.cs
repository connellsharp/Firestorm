using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Data.Models;
using Firestorm.Tests.Examples.Web;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Examples.Basics
{
    public class AutoMapTests : IClassFixture<ExampleFixture<AutoMapTests>>
    {
        private HttpClient HttpClient { get; }

        public AutoMapTests(ExampleFixture<AutoMapTests> fixture)
        {
            HttpClient = fixture.HttpClient;
        }

        [DataSourceRoot]
        public class ArtistsStem : Stem<Artist>
        {
            [Identifier]
            public static int ArtistID { get; set; }

            [Get]
            public static string Name { get; set; }

            [Get]
            public static DateTime StartDate { get; set; }
        }

        [Fact]
        public async Task ArtistsCollection_Get_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ArtistItem_Get_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            object obj = JsonConvert.DeserializeObject(json);

        }

        [Fact]
        public async Task ArtistNameScalar_Get_Correct()
        {
            string responseStr = await HttpClient.GetStringAsync("/artists/1/name");
            Assert.Equal("\"Eminem\"", responseStr);
        }

        [Fact]
        public async Task ArtistsCollection_GetWithFields_DeserialisesAndCorrect()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists?fields=start_date,name");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            string name = obj[0].name;
            Assert.Equal("Eminem", name);
        }
    }
}