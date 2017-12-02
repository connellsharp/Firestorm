using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable;
using Firestorm.Tests.Models;
using Xunit;

namespace Firestorm.Tests.Engine.Fields
{
    public class QueryableFieldSelectorTests
    {
        public QueryableFieldSelectorTests()
        {
            Selector = new QueryableFieldSelector<Artist>(new Dictionary<string, IFieldReader<Artist>> {
                { "name", new PropertyInfoFieldReader<Artist>(typeof(Artist).GetProperty(nameof(Artist.Name))) }
            });
        }

        private QueryableFieldSelector<Artist> Selector { get; set; }

        [Fact]
        public async Task SelectsFieldForSingleItem()
        {
            var artists = TestRepositories.GetArtists().AsQueryable();

            var firstArtist = artists.First();
            var selectedItemData = await Selector.SelectFieldsOnlyAsync(firstArtist);

            Assert.Equal(firstArtist.Name, selectedItemData["name"]);
        }

        [Fact]
        public async Task SelectsFieldForQueryable()
        {
            var artists = TestRepositories.GetArtists().AsQueryable();
            var selectedItemData = await Selector.SelectFieldsOnlyAsync(artists, ItemQueryHelper.DefaultForEachAsync);

            var firstArtist = artists.First();
            var firstSelection = selectedItemData.First();

            Assert.Equal(firstArtist.Name, firstSelection["name"]);
        }
    }
}
