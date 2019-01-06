using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class IdentifierCombinedTests
    {
        private readonly IRestCollection _restCollection;
        
        public IdentifierCombinedTests()
        {
            var testContext = new StemTestContext();
            _restCollection = testContext.GetArtistsCollection<ArtistsStem>();
        }

        private class ArtistsStem : Stem<Artist>
        {
            [Identifier(Name = "id")]
            [Get(Display.Nested)]
            public static Expression<Func<Artist, int>> ID
            {
                get { return a => a.ID; }
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
            var obj = await _restCollection.GetItem("123").GetDataAsync(null);
            Assert.Equal(TestRepositories.ArtistName, obj["Name"].ToString());
        }

        [Fact]
        public async Task ArtistByNameWithoutSpecifiying_GetName_IsCorrect()
        {
            var obj = await _restCollection.GetItem(TestRepositories.ArtistName).GetDataAsync(null);
            Assert.Equal(TestRepositories.ArtistName, obj["Name"]);
        }

        [Fact]
        public async Task ArtistByIDWithSpecifiying_GetName_IsCorrect()
        {
            var obj = await _restCollection.ToDictionary("id").GetItem("123").GetDataAsync(null);
            Assert.Equal(TestRepositories.ArtistName, obj["Name"]);
        }

        [Fact]
        public async Task ArtistByNameWithSpecifiying_GetName_IsCorrect()
        {
            var obj = await _restCollection.ToDictionary("name").GetItem(TestRepositories.ArtistName).GetDataAsync(null);
            Assert.Equal(TestRepositories.ArtistName, obj["Name"]);
        }

        [Fact]
        public async Task ArtistByExactString_GetName_IsCorrect()
        {
            var obj = await _restCollection.GetItem("me").GetDataAsync(null);
            Assert.Equal("My artist", obj["Name"]);
        }
    }
}