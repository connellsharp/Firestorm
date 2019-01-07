using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Tests.Models;
using Firestorm.Testing.Models;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Engine.Tests.Implementation
{
    public class EngineRestDictionaryTests
    {
        private readonly EngineRestDictionary<Artist> _artistDictionary;
        
        public EngineRestDictionaryTests()
        {
            _artistDictionary = new EngineRestDictionary<Artist>(new CodedArtistEntityContext(null), new IdConventionIdentifierInfo<Artist>());
        }

        [Fact]
        public async Task QueryDataAsync_NullQuery_ReturnsID()
        {
            var data = await _artistDictionary.QueryDataAsync(null);
            var itemData = (RestItemData)data.Items["123"];
            Assert.Equal(123, itemData["id"]);
        }

        [Fact]
        public async Task QueryDataAsync_TestQuery_ReturnsCorrectName()
        {
            var data = await _artistDictionary.QueryDataAsync(new TestCollectionQuery
            {
                SelectFields = new List<string> { "name" }
            });

            var itemData = (RestItemData)data.Items["123"];
            Assert.Equal(TestRepositories.ArtistName, itemData["name"]);
        }

        [Fact]
        public async Task GetItem_Get_ReturnsCorrectName()
        {
            var item = _artistDictionary.GetItem("123");
            var itemData = await item.GetDataAsync(null);
            Assert.Equal(TestRepositories.ArtistName, itemData["name"]);
        }
    }
}