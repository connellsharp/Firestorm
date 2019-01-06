using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable;
using Moq;
using Xunit;

namespace Firestorm.Engine.Tests.Queryable
{
    public class QueryableFieldSelectorTests
    {
        private static readonly PropertyInfo NameProperty = typeof(Artist).GetProperty(nameof(Artist.Name));

        private readonly Fixture _fixture;

        private class Artist
        {
            public int ID { get; set; }

            public string Name { get; set; }
        }

        public QueryableFieldSelectorTests()
        {
            _fixture = new Fixture();

        }

        [Fact]
        public async Task SingleItem_SelectFields_CorrectName()
        {
            var artist = _fixture.Create<Artist>();

            var selector = new QueryableFieldSelector<Artist>(new Dictionary<string, IFieldReader<Artist>>
            {
                { "name", new PropertyInfoFieldReader<Artist>(NameProperty) }
            });

            var selectedItemData = await selector.SelectFieldsOnlyAsync(artist);

            Assert.Equal(artist.Name, selectedItemData["name"]);
        }

        [Fact]
        public async Task Queryable_SelectFields_CorrectName()
        {
            var artists = _fixture.CreateMany<Artist>().ToList();

            var selector = new QueryableFieldSelector<Artist>(new Dictionary<string, IFieldReader<Artist>>
            {
                { "name", new PropertyInfoFieldReader<Artist>(NameProperty) }
            });

            var selectedItemData = await selector.SelectFieldsOnlyAsync(artists.AsQueryable(), ItemQueryHelper.DefaultForEachAsync);
            var firstSelection = selectedItemData.First();

            Assert.Equal(artists[0].Name, firstSelection["name"]);
        }

        [Fact]
        public async Task Replacers_SelectFields_CorrectName()
        {
            var artists = _fixture.CreateMany<Artist>().ToList();

            var selector = new QueryableFieldSelector<Artist>(new Dictionary<string, IFieldReader<Artist>>
            {
                { "name", new ReplacerFieldReader(new PropertyInfoFieldReader<Artist>(NameProperty)) }
            });

            var selectedItemData = await selector.SelectFieldsOnlyAsync(artists.AsQueryable(), ItemQueryHelper.DefaultForEachAsync);
            var firstSelection = selectedItemData.First();

            Assert.Equal("replaced", firstSelection["name"]);
        }

        private class ReplacerFieldReader : IFieldReader<Artist>, IFieldValueReplacer<Artist>
        {
            private readonly IFieldReader<Artist> _fieldReaderImplementation;

            public ReplacerFieldReader(IFieldReader<Artist> fieldReaderImplementation)
            {
                _fieldReaderImplementation = fieldReaderImplementation;
            }

            public Type FieldType
            {
                get { return _fieldReaderImplementation.FieldType; }
            }

            public Expression GetSelectExpression(ParameterExpression itemPram)
            {
                return _fieldReaderImplementation.GetSelectExpression(itemPram);
            }

            public IFieldValueReplacer<Artist> Replacer => this;

            public Task LoadAsync(IQueryable<Artist> itemsQuery)
            {
                return Task.CompletedTask;
            }

            public object GetReplacement(object dbValue)
            {
                return "replaced";
            }
        }
    }
}
