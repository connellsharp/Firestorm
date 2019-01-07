using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Testing;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class OverrideEventsTests
    {
        private readonly IRestCollection _restCollection;
        private readonly EventChecker _eventChecker;

        public OverrideEventsTests()
        {
            _eventChecker = new EventChecker();

            var testContext = new StemTestContext();
            testContext.TestDependencyResolver.Add(_eventChecker);
            _restCollection = testContext.GetArtistsCollection<ArtistsStem>();
        }

        private class EventChecker
        {
            public bool OnSavingCalled { get; set; }
            public bool OnSavedCalled { get; set; }
            public bool OnCreatingCalled { get; set; }
            public bool MarkDeletedCalled { get; set; }
            public bool OnUpdatingCalled { get; set; }
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
            
            [Get, Set]
            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }

            public override Task OnSavingAsync(Artist item)
            {
                _eventChecker.OnSavingCalled = true;
                return base.OnSavingAsync(item);
            }

            public override Task OnSavedAsync(Artist item)
            {
                _eventChecker.OnSavedCalled = true;
                return base.OnSavedAsync(item);
            }

            public override void OnCreating(Artist newItem)
            {
                _eventChecker.OnCreatingCalled = true;
                base.OnSavedAsync(newItem);
            }

            public override void OnUpdating(Artist newItem)
            {
                _eventChecker.OnUpdatingCalled = true;
                base.OnUpdating(newItem);
            }

            public override bool MarkDeleted(Artist item)
            {
                _eventChecker.MarkDeletedCalled = true;
                return base.MarkDeleted(item);
            }
        }

        [Fact]
        public async Task Item_Add_OnCreatingWasCalled()
        {
            await _restCollection.AddAsync(new { Name = "Test" });

            Assert.True(_eventChecker.OnCreatingCalled);
        }

        [Fact]
        public async Task Item_Add_OnUpdatingWasNotCalled()
        {
            await _restCollection.AddAsync(new { Name = "Test" });

            Assert.False(_eventChecker.OnUpdatingCalled);
        }

        [Fact]
        public async Task Item_Add_OnSavingWasCalled()
        {
            await _restCollection.AddAsync(new { Name = "Test" });

            Assert.True(_eventChecker.OnSavingCalled);
        }

        [Fact]
        public async Task Item_Add_OnSavedWasCalled()
        {
            await _restCollection.AddAsync(new { Name = "Test" });

            Assert.True(_eventChecker.OnSavedCalled);
        }

        [Fact]
        public async Task Item_Edit_OnUpdateWasCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.True(_eventChecker.OnUpdatingCalled);
        }

        [Fact]
        public async Task Item_Edit_OnCreatingWasNotCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.False(_eventChecker.OnCreatingCalled);
        }

        [Fact]
        public async Task Item_Edit_MarkDeletedWasNotCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.False(_eventChecker.MarkDeletedCalled);
        }

        [Fact]
        public async Task Item_Edit_OnSavingWasCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.True(_eventChecker.OnSavingCalled);
        }

        [Fact]
        public async Task Item_Edit_OnSavedWasCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.True(_eventChecker.OnSavedCalled);
        }

        [Fact]
        public async Task Item_Delete_MarkDeletedWasCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.True(_eventChecker.MarkDeletedCalled);
        }

        [Fact]
        public async Task Item_Delete_OnUpdatingWasNotCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.False(_eventChecker.OnUpdatingCalled);
        }

        [Fact]
        public async Task Item_Delete_OnCreatingWasNotCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.False(_eventChecker.OnCreatingCalled);
        }

        [Fact]
        public async Task Item_Delete_OnSavingWasCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.True(_eventChecker.OnSavingCalled);
        }

        [Fact]
        public async Task Item_Delete_OnSavedWasCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.True(_eventChecker.OnSavedCalled);
        }

        [Fact]
        public async Task Item_Get_OnCreatingWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_eventChecker.OnCreatingCalled);
        }

        [Fact]
        public async Task Item_Get_MarkDeletedWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_eventChecker.MarkDeletedCalled);
        }

        [Fact]
        public async Task Item_Get_OnUpdatingWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_eventChecker.OnUpdatingCalled);
        }

        [Fact]
        public async Task Item_Get_OnSavingWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_eventChecker.OnSavingCalled);
        }

        [Fact]
        public async Task Item_Get_OnSavedWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_eventChecker.OnSavedCalled);
        }

        [Fact]
        public async Task ScalarField_Edit_OnUpdateWasCalled()
        {
            await _restCollection.GetItem("123").GetScalar("Name").EditAsync("Test");

            Assert.True(_eventChecker.OnUpdatingCalled);
        }
    }
}