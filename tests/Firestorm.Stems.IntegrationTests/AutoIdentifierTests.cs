using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Testing;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class AutoIdentifierTests
    {
        private readonly IRestCollection _restCollection;
        
        public AutoIdentifierTests()
        {
            var testContext = new StemTestContext();
            _restCollection = testContext.GetArtistsCollection<ArtistsStem>();
        }

        private class ArtistsStem : Stem<Artist>
        {
            [Get]
            public static Expression<Func<Artist, int>> Id
            {
                get { return a => a.ID; }
            }

            [Get]
            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }
        }

        [Fact]
        public async Task NoCollectionQuery_GetCollectionData_OnlyIdDisplaysInNestedCollection()
        {
            var collectionData = await _restCollection.QueryDataAsync(null);
            
            IEnumerable<string> distinctKeys = collectionData.Items.SelectMany(i => i.Keys).Distinct();
            
            string key = Assert.Single(distinctKeys);
            Assert.Equal("Id", key);
        }

        [Fact]
        public async Task NoCollectionQuery_GeItemData_IdUsedToLocateCorrectItem()
        {
            var itemData = await _restCollection.GetItem("123").GetDataAsync(null);
            
            Assert.Equal(TestRepositories.ArtistName, itemData["Name"]);
        }
    }
}