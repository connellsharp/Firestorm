using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Tests.Functionality.Stems.Helpers;
using Firestorm.Tests.Unit;
using Firestorm.Tests.Unit.Engine.Implementation;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class NestingTests
    {
        private readonly IRestCollection _restCollection;
        
        public NestingTests()
        {
            _restCollection = StemTestUtility.GetArtistsCollection<ArtistsStem>();
        }

        private class ArtistsStem : Stem<Artist>
        {
            [Get(Display.FullItem)]
            public static Expression Id
            {
                get { return Expression(a => a.ID); }
            }

            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }

            [Get]
            [Substem(typeof(ArtistAlbumsStem))]
            public static Expression<Func<Artist, ICollection<Album>>> Albums
            {
                get { return a => a.Albums; }
            }
        }

        private class ArtistAlbumsStem : Stem<Album>
        {
            [Get(Display.Nested)]
            public static Expression<Func<Album, int>> Id
            {
                get { return a => a.Id; }
            }

            [Get(Display.NestedMany)]
            public static Expression<Func<Album, string>> Name
            {
                get { return a => a.Name; }
            }
        }

        [Fact]
        public async Task NestedManyWithoutNestingSubstem_GetCollectionData_DoesntContainNestedSubstem()
        {
            var collectionData = await _restCollection.QueryDataAsync(null);

            var firstItem = collectionData.Items.First();
            
            Assert.Contains(firstItem.Keys, s => s == "Albums");
        }

        [Fact]
        public async Task NestedMany_GetCollectionData_ContainsNestedManyField()
        {
            var collectionData = await _restCollection.QueryDataAsync(new TestCollectionQuery
            {
                SelectFields = new[] { "Albums" }
            });

            var firstItem = collectionData.Items.First();
            var albums = firstItem["Albums"] as IEnumerable<object>;
            dynamic firstAlbum = albums.First();
            
            Assert.Equal(TestRepositories.AlbumName, firstAlbum.Name);
        }

        [Fact(Skip = "Nesting tracking for sub items and collections to be done in a later version.")]
        public async Task NestedOnce_GetCollectionData_DoesntContainIdAtTwoNestingLevels()
        {
            var collectionData = await _restCollection.QueryDataAsync(new TestCollectionQuery
            {
                SelectFields = new[] { "Albums" }
            });

            var firstItem = collectionData.Items.First();
            var albums = firstItem["Albums"] as IEnumerable<object>;
            object firstAlbum = albums.First();

            var members = firstAlbum.GetType().GetMember("Id");
            Assert.Empty(members);
        }

        [Fact]
        public async Task NestedOnce_GetSubCollectionDirectly_ContainsNestedField()
        {
            var itemData = await _restCollection.GetItem("123").GetDataAsync(null);

            var albums = itemData["Albums"] as IEnumerable<object>;
            object firstAlbum = albums.First();

            var members = firstAlbum.GetType().GetMember("Name");
            Assert.NotEmpty(members);
        }
    }
}