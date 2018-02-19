using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Tests.Integration.Data.Base.Models;
using Xunit;

namespace Firestorm.Tests.Integration.Data.Base
{
    public abstract class BasicDataTests
    {
        private readonly EngineRestCollection<Artist> _collection;

        protected BasicDataTests(IDataTransaction transaction, IEngineRepository<Artist> repository)
        {
            _collection = new EngineRestCollection<Artist>(new TestEngineContext(transaction, repository));
        }

        [Fact]
        public async Task GetCollection_NullQuery_ReturnsItems()
        {
            RestCollectionData data = await _collection.QueryDataAsync(null);
            return;
        }

        [Fact]
        public async Task AddItem_NameOnly_NewIdentifierAboveZero()
        {
            var ack = await _collection.AddAsync(new { name = "A new artist" });
            Assert.True((int)ack.NewIdentifier > 0);
        }

        [Fact]
        public async Task GetItem_GetData_DoesntThrow()
        {
            var item = _collection.GetItem(3);
            var data = await item.GetDataAsync(null);
        }

        [Fact]
        public async Task GetItem_Edit_DoesntThrow()
        {
            var item = _collection.GetItem(4);
            var data = await item.EditAsync(new { name = "A changed artist" });
        }

        [Fact]
        public async Task GetIDField__CorrectValue()
        {
            var field = _collection.GetItem(1).GetScalar("id");
            int id = (int)await field.GetAsync();
            Assert.Equal(1, id);
        }

        [Fact]
        public async Task GetNameField__CorrectValue()
        {
            var field = _collection.GetItem(1).GetScalar("name");
            string name = (string)await field.GetAsync();
            Assert.Equal("Eminem", name);
        }

        [Fact]
        public async Task NameField_SetScalar_DoesntThrow()
        {
            var field = _collection.GetItem(1).GetScalar("name");
            var ack1 = await field.EditAsync("Bilbo");
            var ack2 = await field.EditAsync("Eminem");
        }
    }
}
