using System;
using System.Linq.Expressions;
using Firestorm.Stems;
using Firestorm.Stems.Roots.Entities;
using Firestorm.Tests.Examples.Data;
using Firestorm.Tests.Examples.Data.Models;

namespace Firestorm.Tests.Examples.Stems
{
    [EntityRoot]
    public class ArtistsStem : Stem<Artist>
    {
        [Identifier]
        [Get(DisplayFor.NestedOnce)]
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