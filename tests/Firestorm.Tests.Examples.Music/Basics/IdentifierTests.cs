using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Data.Models;
using Firestorm.Tests.Examples.Web;
using Xunit;
using Newtonsoft.Json;

namespace Firestorm.Tests.Examples.Basics
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
            [Identifier(Name = "id")]
            [Get(Display.Nested)]
            public static Expression<Func<Artist, int>> ID
            {
                get { return a => a.ArtistID; }
            }

            [Identifier(Name = "name")]
            [Get]
            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }

            [Identifier(Exactly = "me")]
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
            dynamic obj = JsonConvert.DeserializeObject(json);
            return obj;
        }
    }
}