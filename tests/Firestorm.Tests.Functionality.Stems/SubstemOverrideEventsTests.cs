using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Tests.Functionality.Stems.Helpers;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class SubstemOverrideEventsTests
    {
        private readonly IRestCollection _artistCollection;
        private readonly EventCounter _eventCounter;

        public SubstemOverrideEventsTests()
        {
            _eventCounter = new EventCounter();
            StemTestUtility.TestDependencyResolver.Add(_eventCounter);

            _artistCollection = StemTestUtility.GetArtistsCollection<ArtistsStem>();
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

            public override bool CanAddItem()
            {
                return true;
            }

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

        private Task UpdateAlbumAsync()
        {
            return _artistCollection.GetItem("123").GetCollection("Albums").GetItem("1").EditAsync(new
            {
                Name = "Test album"
            });
        }

        [Fact]
        public async Task ParentStemDependency_Add_OnUpdatingWasNotCount()
        {
            await UpdateAlbumAsync();
            Assert.Equal(0, _eventCounter.OnParentUpdatingCount);
        }

        [Fact]
        public async Task ParentStemDependency_Add_OnSavingWasNotCount()
        {
            await UpdateAlbumAsync();
            Assert.Equal(0, _eventCounter.OnParentSavingCount);
        }

        [Fact]
        public async Task ParentStemDependency_Add_OnSavedWasNotCount()
        {
            await UpdateAlbumAsync();
            Assert.Equal(0, _eventCounter.OnParentSavedCount);
        }

        [Fact]
        public async Task SubstemDependency_Add_OnUpdatingWasCount()
        {
            await UpdateAlbumAsync();
            Assert.Equal(1, _eventCounter.OnChildUpdatingCount);
        }

        [Fact]
        public async Task SubstemDependency_Add_OnSavingWasCount()
        {
            await UpdateAlbumAsync();
            Assert.Equal(1, _eventCounter.OnChildSavingCount);
        }

        [Fact]
        public async Task SubstemDependency_Add_OnSavedWasCount()
        {
            await UpdateAlbumAsync();
            Assert.Equal(1, _eventCounter.OnChildSavedCount);
        }
    }
}