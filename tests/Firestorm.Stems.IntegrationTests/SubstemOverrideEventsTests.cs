using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class SubstemOverrideEventsTests
    {
        private readonly IRestCollection _artistCollection;
        private readonly EventCounter _eventCounter;

        public SubstemOverrideEventsTests()
        {
            _eventCounter = new EventCounter();
            var testContext = new StemTestContext();
            
            testContext.TestDependencyResolver.Add(_eventCounter);

            _artistCollection = testContext.GetArtistsCollection<ArtistsStem>();
        }

        private class EventCounter
        {
            public int OnParentUpdatingCount { get; set; }
            public int OnParentSavingCount { get; set; }
            public int OnParentSavedCount { get; set; }
            public int OnChildSavingCount { get; set; }
            public int OnChildSavedCount { get; set; }
            public int OnChildUpdatingCount { get; set; }
        }

        private class ArtistsStem : Stem<Artist>
        {
            private readonly EventCounter _eventCounter;

            public ArtistsStem(EventCounter eventCounter)
            {
                _eventCounter = eventCounter;
            }

            [Get]
            public static Expression<Func<Artist, int>> ID
            {
                get { return a => a.ID; }
            }

            [Get]
            [Substem(typeof(AlbumSubstem))]
            public static Expression<Func<Artist, ICollection<Album>>> Albums { get; } = a => a.Albums;

            [Get]
            [Substem(typeof(AlbumSubstem))]
            public static Expression<Func<Artist, Album>> FirstAlbum { get; } = a => a.Albums.First();

            public override void OnUpdating(Artist item)
            {
                _eventCounter.OnParentUpdatingCount++;
                base.OnUpdating(item);
            }

            public override Task OnSavingAsync(Artist item)
            {
                _eventCounter.OnParentSavingCount++;
                return base.OnSavingAsync(item);
            }

            public override Task OnSavedAsync(Artist item)
            {
                _eventCounter.OnParentSavedCount++;
                return base.OnSavedAsync(item);
            }
        }

        private class AlbumSubstem : Stem<Album>
        {
            private readonly EventCounter _eventCounter;

            public AlbumSubstem(EventCounter eventCounter)
            {
                _eventCounter = eventCounter;
            }

            [Get(Display.Nested), Identifier]
            public static Expression<Func<Album, int>> ID { get; } = a => a.Id;

            [Get, Set]
            public static Expression<Func<Album, string>> Name { get; } = a => a.Name;

            public override void OnUpdating(Album item)
            {
                _eventCounter.OnChildUpdatingCount++;
                base.OnUpdating(item);
            }

            public override Task OnSavingAsync(Album item)
            {
                _eventCounter.OnChildSavingCount++;
                return base.OnSavingAsync(item);
            }

            public override Task OnSavedAsync(Album item)
            {
                _eventCounter.OnChildSavedCount++;
                return base.OnSavedAsync(item);
            }
        }

        private Task UpdateCollectionAsync()
        {
            return _artistCollection.GetItem("123").GetCollection("Albums").GetItem("1").EditAsync(new
            {
                Name = "Test album"
            });
        }

        private Task UpdateItemAsync()
        {
            return _artistCollection.GetItem("123").GetItem("FirstAlbum").EditAsync(new
            {
                Name = "Test album"
            });
        }

        private Task UpdateScalarAsync()
        {
            return _artistCollection.GetItem("123").GetItem("FirstAlbum").GetScalar("Name").EditAsync("Test album");
        }

        [Fact]
        public async Task ParentStemDependency_Edit_OnUpdatingWasNotCalled()
        {
            await UpdateCollectionAsync();
            Assert.Equal(0, _eventCounter.OnParentUpdatingCount);
        }

        [Fact]
        public async Task ParentStemDependency_Edit_OnSavingWasNotCalled()
        {
            await UpdateCollectionAsync();
            Assert.Equal(0, _eventCounter.OnParentSavingCount);
        }

        [Fact]
        public async Task ParentStemDependency_Edit_OnSavedWasNotCalled()
        {
            await UpdateCollectionAsync();
            Assert.Equal(0, _eventCounter.OnParentSavedCount);
        }

        [Fact]
        public async Task SubCollection_Edit_OnUpdatingWasCalledOnce()
        {
            await UpdateCollectionAsync();
            Assert.Equal(1, _eventCounter.OnChildUpdatingCount);
        }

        [Fact]
        public async Task SubCollection_Edit_OnSavingWasCalledOnce()
        {
            await UpdateCollectionAsync();
            Assert.Equal(1, _eventCounter.OnChildSavingCount);
        }

        [Fact]
        public async Task SubCollection_Edit_OnSavedWasCalledOnce()
        {
            await UpdateCollectionAsync();
            Assert.Equal(1, _eventCounter.OnChildSavedCount);
        }

        [Fact]
        public async Task SubItem_Edit_OnUpdatingWasCalledOnce()
        {
            await UpdateItemAsync();
            Assert.Equal(1, _eventCounter.OnChildUpdatingCount);
        }

        [Fact]
        public async Task SubItem_Edit_OnSavingWasCalledOnce()
        {
            await UpdateItemAsync();
            Assert.Equal(1, _eventCounter.OnChildSavingCount);
        }

        [Fact]
        public async Task SubItem_Edit_OnSavedWasCalledOnce()
        {
            await UpdateItemAsync();
            Assert.Equal(1, _eventCounter.OnChildSavedCount);
        }

        [Fact]
        public async Task SubScalar_Edit_OnUpdatingWasCalledOnce()
        {
            await UpdateScalarAsync();
            Assert.Equal(1, _eventCounter.OnChildUpdatingCount);
        }

        [Fact]
        public async Task SubScalar_Edit_OnSavingWasCalledOnce()
        {
            await UpdateScalarAsync();
            Assert.Equal(1, _eventCounter.OnChildSavingCount);
        }

        [Fact]
        public async Task SubScalar_Edit_OnSavedWasCalledOnce()
        {
            await UpdateScalarAsync();
            Assert.Equal(1, _eventCounter.OnChildSavedCount);
        }
    }
}