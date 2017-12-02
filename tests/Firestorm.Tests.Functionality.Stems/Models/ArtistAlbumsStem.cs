using System;
using System.Linq.Expressions;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Tests.Unit;

namespace Firestorm.Tests.Functionality.Stems.Models
{
    internal class ArtistAlbumsStem : Stem<Album>
    {
        [Get(Display.Nested), Identifier]
        [Locator]
        public static Expression<Func<Album, int>> ID { get; } = a => a.ID;

        [Get, Set]
        public static Expression<Func<Album, string>> Name { get; } = a => a.Name;
    }
}