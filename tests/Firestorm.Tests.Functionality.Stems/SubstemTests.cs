using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Tests.Functionality.Stems.Helpers;
using Firestorm.Tests.Functionality.Stems.Models;
using Firestorm.Tests.Unit;
using Firestorm.Tests.Unit.Endpoints.Stubs;
using Firestorm.Tests.Unit.Engine.Implementation;
using Firestorm.Tests.Unit.Engine.Models;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class SubstemsTests
    {
        private readonly IRestCollection _restCollection;

        public SubstemsTests()
        {
            _restCollection = StemTestUtility.GetArtistsCollection<TestStem>();
        }

        private class TestRoot : EngineRoot<Artist>
        {
            protected override IDataTransaction DataTransaction { get; } = new VoidTransaction();

            protected override IEngineRepository<Artist> Repository { get; } = new ArtistMemoryRepository();

            public override Type StartStemType { get; } = typeof(TestStem);
        }

        private class TestStem : Stem<Artist>
        {
            [Get(Display.Nested), Identifier]
            public static Expression<Func<Artist, int>> ID { get; } = a => a.ID;

            [Get, Set]
            public static Expression<Func<Artist, string>> Name { get; } = a => a.Name;

            [Get(Display.Hidden)]
            [Substem(typeof(AlbumSubstem))]
            public static Expression<Func<Artist, ICollection<Album>>> Albums { get; } = a => a.Albums;
        }

        private class AlbumSubstem : Stem<Album>
        {
            [Get(Display.Nested), Identifier]
            public static Expression<Func<Album, int>> ID { get; } = a => a.Id;

            [Get, Set]
            public static Expression<Func<Album, string>> Name { get; } = a => a.Name;

            [Get]
            [Authorize(Users = "NotThisUser")]
            public static Expression<Func<Album, string>> SecretName { get; } = a => "OMG you got the secret: " + a.Name;

            [Get]
            [Authorize(Users = TestEndpointContext.TestUsername)]
            public static Expression<Func<Album, string>> AllowedName { get; } = a => "You're allowed this one: " + a.Name;

            [Get, Set]
            public static Expression<Func<Album, DateTime>> ReleaseDate { get; } = a => a.ReleaseDate;

            public override bool CanAddItem()
            {
                return true;
            }
        }

        [Fact]
        public async Task GetAlbums_FirstOneCorrectID()
        {
            RestCollectionData albums = await _restCollection.GetItem("123").GetCollection("Albums").QueryDataAsync(null);
            Assert.Equal(1, albums.Items.First()["ID"]);
        }

        [Fact]
        public async Task GetAlbum_CorrectTitle()
        {
            RestItemData album = await _restCollection.GetItem("123").GetCollection("Albums").GetItem("1").GetDataAsync(null);
            Assert.Equal(album["Name"], TestRepositories.AlbumName);
        }

        [Fact]
        public async Task AddAlbum_DoesntThrow()
        {
            var ack = await _restCollection.GetItem("123").GetCollection("Albums").AddAsync(new { Name = "new album" });
        }

        [Fact]
        public async Task GetAlbumSecretName_ThrowsForbidden()
        {
            await Assert.ThrowsAnyAsync<RestApiException>(async delegate
            {
                RestCollectionData albums = await _restCollection.GetItem("123").GetCollection("Albums").QueryDataAsync(new TestCollectionQuery()
                {
                    SelectFields = new [] { "SecretName" }
                });
            });
        }

        [Fact]
        public async Task GetAlbumAllowedName_GetName()
        {
            RestCollectionData albums = await _restCollection.GetItem("123").GetCollection("Albums").QueryDataAsync(new TestCollectionQuery()
            {
                SelectFields = new string[] { "AllowedName" }
            });

            Assert.StartsWith("You're allowed this one: ", albums.Items.First()["AllowedName"].ToString());
        }
    }
}
