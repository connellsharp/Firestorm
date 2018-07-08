using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Tests.Functionality.Stems.Helpers;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class OverrideEventsTests
    {
        private readonly IRestCollection _restCollection;
        private readonly Dependency _dependency;

        public OverrideEventsTests()
        {
            _dependency = new Dependency();
            StemTestUtility.TestDependencyResolver.Add(_dependency);

            _restCollection = StemTestUtility.GetArtistsCollection<ArtistsStem>();
        }

        private class Dependency
        {
            public bool OnSavingCalled { get; set; }
            public bool OnSavedCalled { get; set; }
            public bool OnCreatingCalled { get; set; }
            public bool MarkDeletedCalled { get; set; }
            public bool OnUpdatingCalled { get; set; }
        }

        private class ArtistsStem : Stem<Artist>
        {
            private readonly Dependency _dependency;

            public ArtistsStem(Dependency dependency)
            {
                _dependency = dependency;
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
                _dependency.OnSavingCalled = true;
                return base.OnSavingAsync(item);
            }

            public override Task OnSavedAsync(Artist item)
            {
                _dependency.OnSavedCalled = true;
                return base.OnSavedAsync(item);
            }

            public override void OnCreating(Artist newItem)
            {
                _dependency.OnCreatingCalled = true;
                base.OnSavedAsync(newItem);
            }

            public override void OnUpdating(Artist newItem)
            {
                _dependency.OnUpdatingCalled = true;
                base.OnUpdating(newItem);
            }

            public override bool MarkDeleted(Artist item)
            {
                _dependency.MarkDeletedCalled = true;
                return base.MarkDeleted(item);
            }

            public override bool CanAddItem()
            {
                return true;
            }
        }

        [Fact]
        public async Task TestDependency_Add_OnCreatingWasCalled()
        {
            await _restCollection.AddAsync(new { Name = "Test" });

            Assert.True(_dependency.OnCreatingCalled);
        }

        [Fact]
        public async Task TestDependency_Add_OnUpdatingWasNotCalled()
        {
            await _restCollection.AddAsync(new { Name = "Test" });

            Assert.False(_dependency.OnUpdatingCalled);
        }

        [Fact]
        public async Task TestDependency_Add_OnSavingWasCalled()
        {
            await _restCollection.AddAsync(new { Name = "Test" });

            Assert.True(_dependency.OnSavingCalled);
        }

        [Fact]
        public async Task TestDependency_Add_OnSavedWasCalled()
        {
            await _restCollection.AddAsync(new { Name = "Test" });

            Assert.True(_dependency.OnSavedCalled);
        }

        [Fact]
        public async Task TestDependency_Edit_OnUpdateWasCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.True(_dependency.OnUpdatingCalled);
        }

        [Fact]
        public async Task TestDependency_Edit_OnCreatingWasNotCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.False(_dependency.OnCreatingCalled);
        }

        [Fact]
        public async Task TestDependency_Edit_MarkDeletedWasNotCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.False(_dependency.MarkDeletedCalled);
        }

        [Fact]
        public async Task TestDependency_Edit_OnSavingWasCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.True(_dependency.OnSavingCalled);
        }

        [Fact]
        public async Task TestDependency_Edit_OnSavedWasCalled()
        {
            await _restCollection.GetItem("123").EditAsync(new { Name = "Test" });

            Assert.True(_dependency.OnSavedCalled);
        }

        [Fact]
        public async Task TestDependency_Delete_MarkDeletedWasCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.True(_dependency.MarkDeletedCalled);
        }

        [Fact]
        public async Task TestDependency_Delete_OnUpdatingWasNotCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.False(_dependency.OnUpdatingCalled);
        }

        [Fact]
        public async Task TestDependency_Delete_OnCreatingWasNotCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.False(_dependency.OnCreatingCalled);
        }

        [Fact]
        public async Task TestDependency_Delete_OnSavingWasCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.True(_dependency.OnSavingCalled);
        }

        [Fact]
        public async Task TestDependency_Delete_OnSavedWasCalled()
        {
            await _restCollection.GetItem("123").DeleteAsync();

            Assert.True(_dependency.OnSavedCalled);
        }

        [Fact]
        public async Task TestDependency_Get_OnCreatingWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_dependency.OnCreatingCalled);
        }

        [Fact]
        public async Task TestDependency_Get_MarkDeletedWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_dependency.MarkDeletedCalled);
        }

        [Fact]
        public async Task TestDependency_Get_OnUpdatingWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_dependency.OnUpdatingCalled);
        }

        [Fact]
        public async Task TestDependency_Get_OnSavingWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_dependency.OnSavingCalled);
        }

        [Fact]
        public async Task TestDependency_Get_OnSavedWasNotCalled()
        {
            await _restCollection.GetItem("123").GetDataAsync(null);

            Assert.False(_dependency.OnSavedCalled);
        }
    }
}