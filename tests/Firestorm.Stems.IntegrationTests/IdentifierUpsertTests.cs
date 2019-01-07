using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Testing;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class IdentifierUpsertTests
    {
        private readonly IRestCollection _restCollection;
        
        public IdentifierUpsertTests()
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

            [Identifier(Name = "name"), Get, Set]
            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }

            [Get, Set]
            public static Expression<Func<Artist, string>> Label
            {
                get { return a => a.Label; }
            }
        }

        [Fact]
        public async Task EditArtistWithNewName_CreatesNewArtist()
        {
            var ack = await _restCollection.ToDictionary("name").GetItem("New artist").EditAsync(new RestItemData(new {
                Label = "New label"
            }));

            Assert.IsType<CreatedItemAcknowledgment>(ack);
        }
    }
}