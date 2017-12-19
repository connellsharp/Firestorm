using System;
using System.Linq.Expressions;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Data.Models;

namespace Firestorm.Tests.Examples.Stems
{
    [DataSourceRoot]
    public class ArtistsStem : Stem<Artist>
    {
        [Identifier]
        [Get(Display.Nested)]
        public static Expression<Func<Artist, int>> ID
        {
            get { return a => a.ArtistID; }
        }

        [Get]
        [Set]
        public static Expression<Func<Artist, string>> Name
        {
            get { return a => a.Name; }
        }

        [Get]
        [Set]
        public static Expression<Func<Artist, DateTime>> StartDate
        {
            get { return a => a.StartDate; }
        }

        public override bool CanAddItem()
        {
            // TODO not so happy with this anyway...
            return true;
        }
    }
}