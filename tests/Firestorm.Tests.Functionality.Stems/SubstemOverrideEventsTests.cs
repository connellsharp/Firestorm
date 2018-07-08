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
        private readonly EventChecker _eventChecker;

        public SubstemOverrideEventsTests()
        {
            _eventChecker = new EventChecker();
            StemTestUtility.TestDependencyResolver.Add(_eventChecker);

            _artistCollection = StemTestUtility.GetArtistsCollection<ArtistsStem>();
        }

        private class EventChecker
        {
            public bool OnParentUpdatingCalled { get; set; }
            public bool OnParentSavingCalled { get; set; }
            public bool OnParentSavedCalled { get; set; }
            public bool OnChildSavingCalled { get; set; }
            public bool OnChildSavedCalled { get; set; }
            public bool OnChildUpdatingCalled { get; set; }
        }

        private class ArtistsStem : Stem<Artist>
        {
            private readonly EventChecker _eventChecker;

            public ArtistsStem(EventChecker eventChecker)
            {
                _eventChecker = eventChecker;
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
                _eventChecker.OnParentUpdatingCalled = true;
                base.OnUpdating(item);
            }

            public override Task OnSavingAsync(Artist item)
            {
                _eventChecker.OnParentSavingCalled = true;
                return base.OnSavingAsync(item);
            }

            public override Task OnSavedAsync(Artist item)
            {
                _eventChecker.OnParentSavedCalled = true;
                return base.OnSavedAsync(item);
            }
        }

        private class AlbumSubstem : Stem<Album>
        {
            private readonly EventChecker _eventChecker;

            public AlbumSubstem(EventChecker eventChecker)
            {
                _eventChecker = eventChecker;
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
                _eventChecker.OnChildUpdatingCalled = true;
                base.OnUpdating(item);
            }

            public override Task OnSavingAsync(Album item)
            {
                _eventChecker.OnChildSavingCalled = true;
                return base.OnSavingAsync(item);
            }

            public override Task OnSavedAsync(Album item)
            {
                _eventChecker.OnChildSavedCalled = true;
                return base.OnSavedAsync(item);
            }
        }

        private Task UpdateAlbum()
        {
            return _artistCollection.GetItem("123").GetCollection("Albums").GetItem("1").EditAsync(new
            {
                Name = "Test album"
            });
        }

        [Fact]
        public async Task ParentStemDependency_Add_OnUpdatingWasCalled()
        {
            await UpdateAlbum();
            Assert.True(_eventChecker.OnParentUpdatingCalled);
        }

        [Fact]
        public async Task ParentStemDependency_Add_OnSavingWasCalled()
        {
            await UpdateAlbum();
            Assert.True(_eventChecker.OnParentSavingCalled);
        }

        [Fact]
        public async Task ParentStemDependency_Add_OnSavedWasCalled()
        {
            await UpdateAlbum();
            Assert.True(_eventChecker.OnParentSavedCalled);
        }

        [Fact]
        public async Task SubstemDependency_Add_OnUpdatingWasCalled()
        {
            await UpdateAlbum();
            Assert.True(_eventChecker.OnChildUpdatingCalled);
        }

        [Fact]
        public async Task SubstemDependency_Add_OnSavingWasCalled()
        {
            await UpdateAlbum();
            Assert.True(_eventChecker.OnChildSavingCalled);
        }

        [Fact]
        public async Task SubstemDependency_Add_OnSavedWasCalled()
        {
            await UpdateAlbum();
            Assert.True(_eventChecker.OnChildSavedCalled);
        }
    }
}