using System;
using System.Linq.Expressions;
using System.Net;
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
    public class SettersTests : IClassFixture<ExampleFixture<SettersTests>>
    {
        private HttpClient HttpClient { get; }

        public SettersTests(ExampleFixture<SettersTests> fixture)
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
            [Set]
            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }

            [Get]
            [Set]
            public static Expression<Func<Artist, DateTime>> StartDate
            {
                get { return a => a.StartDate; }
            }
        }

        [Fact]
        public async Task ArtistsCollection_PostNew_StatusCreated()
        {
            HttpResponseMessage response = await HttpClient.PostAsync("/artists", new JsonContent(@"{ ""name"": ""Fred"", start_date: ""1990-01-01"" }"));

            ResponseAssert.Success(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task ArtistItem_UpdateName_GetIsSame()
        {
            HttpResponseMessage response1 = await HttpClient.PutAsync("/artists/2", new JsonContent(@"{ ""name"": ""Bill"" }"));
            ResponseAssert.Success(response1);

            HttpResponseMessage response2 = await HttpClient.GetAsync("/artists/2");
            ResponseAssert.Success(response2);
            string json = await response2.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);
            Assert.Equal("Bill", obj.name.ToString());
        }

        [Fact]
        public async Task ArtistName_Update_GetIsSame()
        {
            HttpResponseMessage response1 = await HttpClient.PutAsync("/artists/2/name", new JsonContent(@"""Bob"""));
            ResponseAssert.Success(response1);

            HttpResponseMessage response2 = await HttpClient.GetAsync("/artists/2/name");
            ResponseAssert.Success(response2);
            string json = await response2.Content.ReadAsStringAsync();
            Assert.Equal("\"Bob\"", json);
        }
    }
}