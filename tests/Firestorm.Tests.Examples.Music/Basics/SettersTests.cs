using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Music.Data.Models;
using Firestorm.Tests.Examples.Music.Web;ft.Json;
using Xunit;

namespace Firestorm.Tests.Examples.Music.Basics
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
            HttpResponseMessage response = await HttpClient.PostAsync("/artists", JsonContent(@"{ ""name"": ""Fred"", start_date: ""1990-01-01"" }"));

            ResponseAssert.Success(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task ArtistItem_UpdateName_GetIsSame()
        {
            HttpResponseMessage response1 = await HttpClient.PutAsync("/artists/2", JsonContent(@"{ ""name"": ""Bill"" }"));
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
            HttpResponseMessage response1 = await HttpClient.PutAsync("/artists/2/name", JsonContent(@"""Bob"""));
            ResponseAssert.Success(response1);

            HttpResponseMessage response2 = await HttpClient.GetAsync("/artists/2/name");
            ResponseAssert.Success(response2);
            string json = await response2.Content.ReadAsStringAsync();
            Assert.Equal("\"Bob\"", json);
        }

        private static HttpContent JsonContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}