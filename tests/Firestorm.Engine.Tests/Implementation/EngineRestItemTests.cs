﻿using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Tests.Models;
using Firestorm.Testing.Models;
using Firestorm.Testing;
using Xunit;

namespace Firestorm.Engine.Tests.Implementation
{
    public class EngineRestItemTests
    {
        private readonly CodedArtistEntityContext _context;
        private readonly EngineRestItem<Artist> _item;
        private readonly Artist _actualItem;
        
        public EngineRestItemTests()
        {
            _context = new CodedArtistEntityContext(null);
            var idInfo = new IdConventionIdentifierInfo<Artist>();
            _item = new EngineRestItem<Artist>(_context, new IdentifiedItem<Artist>("123", _context.Data.Repository, idInfo, _context.Data.AsyncQueryer));
            _actualItem = _context.Data.Repository.GetAllItems().SingleOrDefault();
        }

        [Fact]
        public async Task GetFieldTest()
        {
            var field = _item.GetField("name");
            var scalar = (IRestScalar) field;
            var value = await scalar.GetAsync();
            Assert.Equal(TestRepositories.ArtistName, value);
        }

        [Fact]
        public async Task EditAsyncTest()
        {
            var ack = await _item.EditAsync(new RestItemData { { "name", "Fred" }});
            Assert.Equal("Fred", _actualItem.Name);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var ack = await _item.DeleteAsync();
            Assert.Equal(0, _context.Data.Repository.GetAllItems().Count());
        }

        [Fact]
        public async Task GetDataAsyncTest()
        {
            var data = await _item.GetDataAsync(null);
            Assert.Equal(TestRepositories.ArtistName, data["name"]);
        }
    }
}