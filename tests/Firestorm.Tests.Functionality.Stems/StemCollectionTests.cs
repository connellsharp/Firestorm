using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Tests.Functionality.Stems.Models;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class StemCollectionTests
    {
        private readonly IRestCollection _restCollection;

        public StemCollectionTests()
        {
            _restCollection = StemTestUtility.GetArtistsCollection<ArtistsStem>();
        }

        [Fact]
        public async Task IdentifiedItem_GetIDOnly_Correct()
        {
            var itemData = await _restCollection.GetItem("123").GetDataAsync(new[] { "ID" });

            Assert.Equal(123, itemData["ID"]);
        }

        [Fact]
        public async Task IdentifiedItem_GetIDField_Correct()
        {
            object scalar = await _restCollection.GetItem("123").GetScalar("ID").GetAsync();

            Assert.Equal(123, scalar);
        }

        [Fact]
        public async Task IdentifiedItem_GetNameField_Correct()
        {
            object scalar = await _restCollection.GetItem("123").GetScalar("Name").GetAsync();

            Assert.Equal(scalar, TestRepositories.ArtistName);
        }

        [Fact]
        public async Task IdentifiedItem_GetFullItem_ContainsIDAndName()
        {
            var itemData = await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.Equal(123, itemData["ID"]);
            Assert.Equal(itemData["Name"], TestRepositories.ArtistName);
            Assert.False(itemData.ContainsKey("Albums"));
        }

        [Fact]
        public async Task IdentifiedItem_GetSubStemAlbums_DoesntError()
        {
            var itemData = await _restCollection.GetItem("123").GetDataAsync(new[] { "Albums" });
            
            Assert.False(itemData.ContainsKey("ID"));
            Assert.False(itemData.ContainsKey("Name"));
            Assert.True(itemData.ContainsKey("Albums"));
        }

        [Fact]
        public async Task GetCollectionOnlyDisplaysNestedFields()
        {
            var idFilterQuery = new SimpleFilterQuery(new FilterInstruction("ID", FilterComparisonOperator.Equals, "123"));
            var collectionData = await _restCollection.QueryDataAsync(idFilterQuery);
            var itemData = collectionData.Items.Single();

            Assert.Equal(123, itemData["ID"]);
            Assert.False(itemData.ContainsKey("Name"));
        }

        private class SimpleFilterQuery : IRestCollectionQuery
        {
            public SimpleFilterQuery(params FilterInstruction[] filterInstructions)
            {
                FilterInstructions = filterInstructions;
            }

            public IEnumerable<string> SelectFields { get; } = null;
            public IEnumerable<FilterInstruction> FilterInstructions { get; }
            public IEnumerable<SortInstruction> SortInstructions { get; } = null;
            public PageInstruction PageInstruction { get; } = null;
        }
    }
}
