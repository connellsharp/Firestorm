using System;
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
    public class SoftDeleteTests
    {
        private readonly IRestCollection _restCollection;
        private readonly TestDeletedValue _testDeletedValue;
        private readonly StemTestContext _testContext;

        public SoftDeleteTests()
        {
            _testDeletedValue = new TestDeletedValue();
            _testContext = new StemTestContext();
            
            _testContext.TestDependencyResolver.Add(_testDeletedValue);

            _restCollection = _testContext.GetArtistsCollection<ArtistsStem>();
        }

        private class TestDeletedValue
        {
            public bool SoftDeleted { get; set; }
        }

        private class ArtistsStem : Stem<Artist>
        {
            private readonly TestDeletedValue _testDeletedValue;

            public ArtistsStem(TestDeletedValue testDeletedValue)
            {
                _testDeletedValue = testDeletedValue;
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

            public override bool MarkDeleted(Artist item)
            {
                return _testDeletedValue.SoftDeleted;
            }

            public override bool CanAddItem()
            {
                return true;
            }
        }

        [Fact]
        public async Task NotSoftDeleted_WhenDeleted_RepoMarkDeletedCalled()
        {
            _testDeletedValue.SoftDeleted = false;
            
            await _restCollection.GetItem("123").DeleteAsync();
            
            bool artistInRepo = _testContext.TestRepository.GetAllItems().Any(a => a.ID == 123);
            Assert.False(artistInRepo);
        }

        [Fact]
        public async Task SoftDeleted_WhenDeleted_RepoMarkDeletedNotCalled()
        {
            _testDeletedValue.SoftDeleted = true;
            
            await _restCollection.GetItem("123").DeleteAsync();

            bool artistInRepo = _testContext.TestRepository.GetAllItems().Any(a => a.ID == 123);
            Assert.True(artistInRepo);
        }
    }
}