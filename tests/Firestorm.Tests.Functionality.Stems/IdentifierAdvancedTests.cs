using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Tests.Functionality.Stems.Helpers;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class IdentifierAdvancedTests
    {
        private readonly IRestCollection _restCollection;
        
        public IdentifierAdvancedTests()
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

            [Identifier(Name = "thisthing")]
            public Artist FindByThisThing(string thisThing)
            {
                return new Artist { Name = "Found by thing: " + thisThing };
            }

            [Identifier(Name = "first_letter")]
            public static Expression<Func<Artist, char>> FirstLetter
            {
                get { return a => a.Name.ToLowerInvariant()[0]; }
            }

            [Identifier]
            public static Expression<Func<Artist, bool>> FindByStartsWith(string prefix)
            {
                return a => a.Name.StartsWith(prefix);
            }
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

        [Theory]
        [InlineData("This is a thing")]
        [InlineData("This is also a thing")]
        public async Task ArtistByInstanceLocator_GetName_IsCorrect(string thisThing)
        {
            var obj = await _restCollection.ToDictionary("thisthing").GetItem(thisThing).GetDataAsync(null);
            Assert.Equal("Found by thing: " + thisThing, obj["Name"]);
        }

        //[Fact]
        //public void ArtistByInstanceBadIdentifier_GetName_Throws()
        //{
        //    Assert.Throws<StemAttributeSetupException>(delegate
        //    {
        //        var obj = _restCollection.ToDictionary("throwsomething");
        //    });
        //}

        [Fact]
        public async Task ArtistByExactString_GetName_IsCorrect()
        {
            var obj = await _restCollection.GetItem("me").GetDataAsync(null);
            Assert.Equal("My artist", obj["Name"]);
        }

        [Fact]
        public async Task ArtistByFirstLetter_GetName_IsCorrect()
        {
            var obj = await _restCollection.ToDictionary("first_letter").GetItem("n").GetDataAsync(null);
            Assert.Equal(TestRepositories.ArtistName, obj["Name"]);
        }

        [Fact]
        public async Task ArtistByStartsWith_GetName_IsCorrect()
        {
            var obj = await _restCollection.ToDictionary("StartsWith").GetItem("Noi").GetDataAsync(null);
            Assert.Equal(TestRepositories.ArtistName, obj["Name"]);
        }
    }
}