using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel;
using Xunit;

namespace Firestorm.Tests.Unit.Stems.Substems
{
    public class SubCollectionFieldWriterTests
    {
        public SubCollectionFieldWriterTests()
        {
            var subStem = new ArtistAlbumsStem();
            subStem.SetParent(new TestStartAxis());

            StemsEngineSubContext<Album> stemsEngineSubContext = new StemsEngineSubContext<Album>(subStem);
            var tools = new SubWriterTools<Artist, IEnumerable<Album>, Album>(a => a.Albums, null, null);
            NameFieldWriter = new SubCollectionFieldWriter<Artist, IEnumerable<Album>, Album>(tools, stemsEngineSubContext);
        }

        public class ArtistAlbumsStem : Stem<Album>
        {
            [Get(Display.Nested), Identifier]
            [Locator]
            public static Expression<Func<Album, int>> Id { get; } = a => a.Id;

            [Get, Set]
            public static Expression<Func<Album, string>> Name { get; } = a => a.Name;
        }

        private SubCollectionFieldWriter<Artist, IEnumerable<Album>, Album> NameFieldWriter { get; set; }

        [Fact]
        public async Task ArtistWithNoAlbums_SetWithoutIDs_CreatesNew()
        {
            var artist = new Artist() { Albums = new List<Album>() };

            var albumsPostedObj = new[]
            {
                new { Name = "First album" },
                new { Name = "Last album" },
            };

            await NameFieldWriter.SetValueAsync(artist, albumsPostedObj, new VoidTransaction());

            Assert.Equal(2, artist.Albums.Count);
            Assert.Equal("First album", artist.Albums.First().Name);
            Assert.Equal("Last album", artist.Albums.Last().Name);
        }

        [Fact]
        public async Task ArtistWithAlbums_SetWithLocatorIDs_Updates()
        {
            // note - test relies on IDs having the locator attribute, which they don't

            var artist = new Artist
            {
                Albums = new List<Album>
                {
                    new Album { Id = 1, Name = "Old first album name" },
                    new Album { Id = 2, Name = "Old second album name" },
                }
            };

            var albumsPostedObj = new[]
            {
                new { id = 1, name = "First album" },
                new { id = 2, name = "Last album" },
            };

            await NameFieldWriter.SetValueAsync(artist, albumsPostedObj, new VoidTransaction());

            Assert.Equal(2, artist.Albums.Count);
            Assert.Equal("First album", artist.Albums.First().Name);
            Assert.Equal("Last album", artist.Albums.Last().Name);
        }
    }
}
