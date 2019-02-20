using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.FunctionalTests.Models;
using Firestorm.Stems.FunctionalTests.Web;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Testing.Http;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Stems.FunctionalTests.Basics
{
    public class GettersAndQueryTests : IClassFixture<ExampleFixture<GettersAndQueryTests>>
    {
        private HttpClient HttpClient { get; }

        public GettersAndQueryTests(ExampleFixture<GettersAndQueryTests> fixture)
        {
            HttpClient = fixture.HttpClient;
        }

        [DataSourceRoot]
        public class ArtistsStem : Stem<Artist>
        {
            [Identifier]
            [Get(Display.Nested)]
            public static Expression<Func<Artist, int>> ID
            {
                get { return a => a.ArtistID; }
            }

            [Get]
            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }

            [Get]
            public static Expression<Func<Artist, DateTime>> StartDate
            {
                get { return a => a.StartDate; }
            }
        }

        [Fact]
        public async Task ArtistsCollection_Get_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists");

            ResponseAssert.Success(response);
        }

        [Fact]
        public async Task ArtistItem_Get_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists/1");

            ResponseAssert.Success(response);

            string json = await response.Content.ReadAsStringAsync();
            object obj = JsonConvert.DeserializeObject(json);

        }

        [Fact]
        public async Task ArtistIDScalar_Get_Correct()
        {
            string responseStr = await HttpClient.GetStringAsync("/artists/1/id");
            Assert.Equal("1", responseStr);
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
            HttpResponseMessage response = await HttpClient.GetAsync("/artists?fields=id,name");

            ResponseAssert.Success(response);

            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            string name = obj[0].name;
            Assert.Equal("Eminem", name);
        }
        
        [Fact]
        public async Task ArtistsCollection_SortByDate_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists?fields=id,name&sort=start_date");

            ResponseAssert.Success(response);

            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            string name = obj[0].name;
            Assert.Equal("Infected Mushroom", name);
        }

        [Fact]
        public async Task ArtistsCollection_FilterAboveID_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists?fields=name&where=id>2");

            ResponseAssert.Success(response);

            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            string name = obj[0].name;
            Assert.Equal("Periphery", name);
        }
    }
}