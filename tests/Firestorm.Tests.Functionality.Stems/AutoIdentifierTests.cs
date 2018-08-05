using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Tests.Functionality.Stems.Helpers;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class AutoIdentifierTests
    {
        private readonly IRestCollection _restCollection;
        
        public AutoIdentifierTests()
        {
            _restCollection = StemTestUtility.GetArtistsCollection<ArtistsStem>();
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