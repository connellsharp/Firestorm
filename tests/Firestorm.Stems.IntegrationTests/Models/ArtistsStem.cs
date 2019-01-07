using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Stems;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials;
using Firestorm.Testing;
using Firestorm.Testing.Models;

namespace Firestorm.Stems.IntegrationTests.Models
{
    internal class ArtistsStem : Stem<Artist>
    {
        [Get(Display.Nested)]
        [Identifier]
        //[Locator]
        public static Expression<Func<Artist, int>> ID { get; } = a => a.ID;

        [Get, Set]
        public static Expression<Func<Artist, string>> Name { get; } = a => a.Name;

        [Get(Display.Hidden), Set]
        [Substem(typeof(ArtistAlbumsStem))]
        public static Expression<Func<Artist, IEnumerable<Album>>> Albums { get; } = a => a.Albums;
    }
}