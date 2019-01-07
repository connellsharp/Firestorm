using System.Collections.Generic;
using System.Linq;
using Firestorm.Engine.Queryable;
using Firestorm.Engine.Tests.Models;
using Firestorm.Testing.Engine;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Engine.Tests.Queryable
{
    public class QueryableFieldFilterTests
    {
        [Fact]
        public void EqualsFilter_Name_OnlyThatOne()
        {
            var filter = new QueryableFieldFilter<Artist>(new FieldDictionary<Artist>
            {
                { "name", a => a.Name }
            }, new List<FilterInstruction>
            {
                new FilterInstruction("name", FilterComparisonOperator.Equals, "Right Said Fred")
            });

            var artists = GetFakeArtistsList();

            var filtered = filter.ApplyFilter(artists.AsQueryable());
            Assert.Equal(1, filtered.Count());
            Assert.Equal("Right Said Fred", filtered.First().Name);
        }

        [Theory]
        [InlineData(250)]
        [InlineData(500)]
        [InlineData(750)]
        public void GreaterThanFilter_ID_OnlyAboveID(int minId)
        {
            var filter = new QueryableFieldFilter<Artist>(new FieldDictionary<Artist>
            {
                { "id", a => a.ID }
            }, new List<FilterInstruction>
            {
                new FilterInstruction("id", FilterComparisonOperator.GreaterThan, minId.ToString())
            });

            var artists = GetFakeArtistsList();

            var filtered = filter.ApplyFilter(artists.AsQueryable());
            Assert.Equal(artists.Count(a => a.ID > minId), filtered.Count());
        }

        [Fact]
        public void ContainsFilter_Fred_FiltersNonFreds()
        {
            var filter = new QueryableFieldFilter<Artist>(new FieldDictionary<Artist>
            {
                { "name", a => a.Name }
            }, new List<FilterInstruction>
            {
                new FilterInstruction("name", FilterComparisonOperator.Contains, "Fred")
            });

            var artists = GetFakeArtistsList();

            var filtered = filter.ApplyFilter(artists.AsQueryable());
            Assert.Equal(4, filtered.Count());
        }

        [Fact]
        public void IsInFilter_BandList_FiltersToThose()
        {
            var filter = new QueryableFieldFilter<Artist>(new FieldDictionary<Artist>
            {
                { "name", a => a.Name }
            }, new List<FilterInstruction>
            {
                new FilterInstruction("name", FilterComparisonOperator.IsIn, "Bill's band featuring Davey McDave")
            });

            var artists = GetFakeArtistsList();

            var filtered = filter.ApplyFilter(artists.AsQueryable());
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void StartsWithFilter_Fred_FiltersToThose()
        {
            var filter = new QueryableFieldFilter<Artist>(new FieldDictionary<Artist>
            {
                { "name", a => a.Name }
            }, new List<FilterInstruction>
            {
                new FilterInstruction("name", FilterComparisonOperator.StartsWith, "Fred")
            });

            var artists = GetFakeArtistsList();

            var filtered = filter.ApplyFilter(artists.AsQueryable());
            Assert.Equal(3, filtered.Count());
        }

        [Fact]
        public void EndsWithFilter_Fred_FiltersToThose()
        {
            var filter = new QueryableFieldFilter<Artist>(new FieldDictionary<Artist>
            {
                { "name", a => a.Name }
            }, new List<FilterInstruction>
            {
                new FilterInstruction("name", FilterComparisonOperator.EndsWith, "Fred")
            });

            var artists = GetFakeArtistsList();

            var filtered = filter.ApplyFilter(artists.AsQueryable());
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void NotEqualsFilter_Fred_FiltersFreds()
        {
            var filter = new QueryableFieldFilter<Artist>(new FieldDictionary<Artist>
            {
                { "name", a => a.Name }
            }, new List<FilterInstruction>
            {
                new FilterInstruction("name", FilterComparisonOperator.NotEquals, "Fred")
            });

            var artists = GetFakeArtistsList();

            var filtered = filter.ApplyFilter(artists.AsQueryable());
            Assert.Equal(5, filtered.Count());
        }

        private static List<Artist> GetFakeArtistsList()
        {
            return new List<Artist>
            {
                new Artist(432, "Fred"),
                new Artist(432, "Fred Durst"),
                new Artist(432, "Right Said Fred"),
                new Artist(543, "Fred's band"),
                new Artist(678, "Davey McDave"),
                new Artist(765, "Bill's band"),
            };
        }
    }
}