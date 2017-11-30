using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Fuel;
using Firestorm.Tests.Models;
using Xunit;

namespace Firestorm.Tests.Stems.Substems
{
    public class SubItemFieldWriterTests
    {
        private readonly SubItemFieldWriter<Album, Artist> _writer;
        
        public SubItemFieldWriterTests()
        {
            var artistsStem = new ArtistsStem();
            artistsStem.SetParent(new TestStartAxis());

            _writer = new SubItemFieldWriter<Album, Artist>(a => a.Artist, new StemsEngineSubContext<Artist>(artistsStem));
        }

        public class ArtistsStem : Stem<Artist>
        {
            // TODO testing too much here, maybe move some of these to one in the functional tests too?

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
        }

        [Fact]
        public async Task AlbumWithArtist_SetArtistName_EditsExistingName()
        {
            var album = new Album { Artist = new Artist(654, "Old guy") };
            await _writer.SetValueAsync(album, new { name = "New guy" }, new TestTransaction());
            Assert.Equal("New guy", album.Artist.Name);
        }

        [Fact(Skip = "Not implemented setting related stems using locators yet.")]
        public async Task AlbumWithNoArtist_SetArtistID_AddsRelationship()
        {
            var album = new Album();
            await _writer.SetValueAsync(album, new { id = 123 }, new TestTransaction());
            // TODO not implemented yet, but this could find the artist with that ID using the Locator
            Assert.Equal(123, album.Artist.ID);
        }

        [Fact]
        public async Task AlbumWithNoArtist_SetArtistName_ThrowsNullReference()
        {
            var album = new Album();
            await Assert.ThrowsAsync<ArgumentNullException>(async delegate
            {
                await _writer.SetValueAsync(album, new { name = "New guy" }, new TestTransaction());
            });
        }
    }
}