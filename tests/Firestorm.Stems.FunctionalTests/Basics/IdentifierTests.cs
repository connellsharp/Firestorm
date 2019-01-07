using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.FunctionalTests.Data.Models;
using Firestorm.Stems.FunctionalTests.Web;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Testing.Http;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Stems.FunctionalTests.Basics
{
    public class IdentifierTests : IClassFixture<ExampleFixture<IdentifierTests>>
    {
        private HttpClient HttpClient { get; }

        public IdentifierTests(ExampleFixture<IdentifierTests> fixture)
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

            [Identifier]
            [Get]
            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }

            [Identifier(Exactly = "me")] // TODO casing?
            public Artist GetExactArtist()
            {
                return new Artist { Name = "My artist" };
            }
        }

        [Fact]
        public async Task ArtistByIDWithoutSpecifiying_GetName_IsCorrect()
        {
            dynamic obj = await GetDynamicObject("/artists/3");
            Assert.Equal("Periphery", obj.name.ToString());
        }

        [Fact]
        public async Task ArtistByNameWithoutSpecifiying_GetName_IsCorrect()
        {
            dynamic obj = await GetDynamicObject("/artists/Periphery");
            Assert.Equal("Periphery", obj.name.ToString());
        }

        [Fact]
        public async Task ArtistByIDWithSpecifiying_GetName_IsCorrect()
        {
            dynamic obj = await GetDynamicObject("/artists/by_id/3");
            Assert.Equal("Periphery", obj.name.ToString());
        }

        [Fact]
        public async Task ArtistByNameWithSpecifiying_GetName_IsCorrect()
        {
            dynamic obj = await GetDynamicObject("/artists/by_name/Periphery");
            Assert.Equal("Periphery", obj.name.ToString());
        }

        [Fact]
        public async Task ArtistByExactString_GetName_IsCorrect()
        {
            dynamic obj = await GetDynamicObject("/artists/me");
            Assert.Equal("My artist", obj.name.ToString());
        }

        private async Task<dynamic> GetDynamicObject(string requestUri)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(requestUri);
            string json = await response.Content.ReadAsStringAsync();

            ResponseAssert.Success(response);

            dynamic obj = JsonConvert.DeserializeObject(json);
            return obj;
        }
    }
}