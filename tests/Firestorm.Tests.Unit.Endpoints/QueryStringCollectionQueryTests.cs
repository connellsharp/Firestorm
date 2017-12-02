using System.Collections.Specialized;
using System.Linq;
using Firestorm.Core;
using Firestorm.Endpoints.Query;
using Xunit;

namespace Firestorm.Tests.Endpoints
{
    public class QueryStringCollectionQueryTests
    {
        [Fact]
        public void GivenFieldsString_ContainsFields()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration(), "fields=id,test,foo");

            Assert.True(query.SelectFields.Contains("id"));
            Assert.True(query.SelectFields.Contains("test"));
            Assert.True(query.SelectFields.Contains("foo"));
        }

        [Fact]
        public void GivenFieldsStringWithQuertionMark_ContainsFields()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration(), "?fields=id,test,foo");

            Assert.True(query.SelectFields.Contains("id"));
            Assert.True(query.SelectFields.Contains("test"));
            Assert.True(query.SelectFields.Contains("foo"));
        }

        [Fact]
        public void GivenFieldsCollection_ContainsFields()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration(), new NameValueCollection {
                { "fields", "test" },
                { "where", "age>25" },
                });
            
            Assert.True(query.SelectFields.Contains("test"));
            Assert.True(query.FilterInstructions.Any(fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.GreaterThan && fi.ValueString == "25"));
        }

        [Fact]
        public void GivenFilters_ContainsFilters()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration(), "where=age>=25&where=age<45");

            Assert.True(query.FilterInstructions.Any(fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.GreaterThanOrEquals && fi.ValueString == "25"));
            Assert.True(query.FilterInstructions.Any(fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.LessThan && fi.ValueString == "45"));
        }

        [Fact]
        public void SpecialKeysEnabled_GivenSpecialFilterKeys_ContainsFilters()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration
            {
                SpecialFilterKeysEnabled = true
            }, "age>=25&age<45");

            Assert.True(query.FilterInstructions.Any(fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.GreaterThanOrEquals && fi.ValueString == "25"));
            Assert.True(query.FilterInstructions.Any(fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.LessThan && fi.ValueString == "45"));
        }

        [Fact]
        public void SpecialKeysDisabled_GivenSpecialFilterKeys_DoesntContainFilters()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration
            {
                SpecialFilterKeysEnabled = false
            }, "age>=25&age<45");

            Assert.Null(query.FilterInstructions);
        }

        [Fact]
        public void GivenSortStringWithComma_ContainsSortInstructions()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration(), "sort=someDate+asc,another+desc");

            Assert.True(query.SortIntructions.Any(si => si.FieldName == "someDate" && si.Direction == SortDirection.Ascending));
            Assert.True(query.SortIntructions.Any(si => si.FieldName == "another" && si.Direction == SortDirection.Descending));
        }

        [Fact]
        public void GivenSortStrings_ContainsSortInstructions()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration(), "sort=someDate+asc&sort=another+desc");

            Assert.True(query.SortIntructions.Any(si => si.FieldName == "someDate" && si.Direction == SortDirection.Ascending));
            Assert.True(query.SortIntructions.Any(si => si.FieldName == "another" && si.Direction == SortDirection.Descending));
        }

        [Fact]
        public void GivenNoSpecificSortDirection_ContainsUnspecified()
        {
            var query = new QueryStringCollectionQuery(new CollectionQueryStringConfiguration(), "sort=column");

            Assert.True(query.SortIntructions.Any(si => si.FieldName == "column" && si.Direction == SortDirection.Unspecified));
        }
    }
}
