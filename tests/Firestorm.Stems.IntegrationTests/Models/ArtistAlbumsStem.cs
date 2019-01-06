using System;
using System.Linq.Expressions;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Tests.Unit;

namespace Firestorm.Stems.IntegrationTests.Models
{
    internal class ArtistAlbumsStem : Stem<Album>
    {
        [Get(Display.Nested), Identifier]
        [Locator]
        public static Expression<Func<Album, int>> Id { get; } = a => a.Id;

        [Get, Set]
        public static Expression<Func<Album, string>> Name { get; } = a => a.Name;
    }
}