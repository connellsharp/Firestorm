using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Tests.Models;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Engine.Tests
{
    public class DeferredItemTests
    {
        [Fact]
        public async Task ConstructNewItem()
        {
            var artist = new Artist();
            var postedNewItem = new PostedNewItem<Artist>(artist);
            await postedNewItem.LoadAsync();
        }

        [Fact]
        public async Task LoadingIdentifiedItem_WithoutExistingID_CreatesNewItem()
        {
            var repo = new ArtistMemoryRepository();
            const int testID = 654321;

            Artist artist = repo.GetAllItems().FirstOrDefault(a => a.ID == testID);
            Assert.Null(artist);

            var postedNewItem = new IdentifiedItem<Artist>(testID.ToString(), repo, new IdConventionIdentifierInfo<Artist>(), new MemoryAsyncQueryer());
            await postedNewItem.LoadAsync();

            Assert.Equal(LazyState.Created, postedNewItem.LazyState);
            Assert.NotNull(postedNewItem.LoadedItem);
        }

        [Fact]
        public async Task LoadingIdentifiedItem_WithExistingID_LoadsExistingItem()
        {
            var repo = new ArtistMemoryRepository();
            const int testID = 123;

            Artist artist = repo.GetAllItems().FirstOrDefault(a => a.ID == testID);
            Assert.NotNull(artist);

            var postedNewItem = new IdentifiedItem<Artist>(testID.ToString(), repo, new IdConventionIdentifierInfo<Artist>(), new MemoryAsyncQueryer());
            await postedNewItem.LoadAsync();

            Assert.Equal(LazyState.LoadedItem, postedNewItem.LazyState);
            Assert.Equal(postedNewItem.LoadedItem, artist);
        }
    }
}
