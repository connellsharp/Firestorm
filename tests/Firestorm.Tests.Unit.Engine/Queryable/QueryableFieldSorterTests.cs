using System.Collections.Generic;
using System.Linq;
using Firestorm.Engine.Queryable;
using Firestorm.Tests.Unit.Engine.Models;
using Xunit;

namespace Firestorm.Tests.Unit.Engine.Queryable
{
    public class QueryableFieldSorterTests
    {
        [Fact]
        public void Writer_ApplySortOrder_InOrder()
        {
            var fieldSorter = new QueryableFieldSorter<Artist>(new FieldDictionary<Artist>
            {
                { "id", a => a.ID }
            }, new[]
            {
                new SortIntruction("id", SortDirection.Ascending)
            });

            var unordered = new List<Artist>
            {
                new Artist(12, "Periphery"),
                new Artist(3, "Noisia"),
                new Artist(78, "Mefjus"),
                new Artist(43, "Drewsif Stalin"),
                new Artist(24, "Tesseract")
            };

            var ordered = fieldSorter.ApplySortOrder(unordered.AsQueryable()).ToList();

            Assert.True(ordered[0].ID < ordered[1].ID);
            Assert.True(ordered[1].ID < ordered[2].ID);
            Assert.True(ordered[2].ID < ordered[3].ID);
            Assert.True(ordered[3].ID < ordered[4].ID);
        }
    }
}