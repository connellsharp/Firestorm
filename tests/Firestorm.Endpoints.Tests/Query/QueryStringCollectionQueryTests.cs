using System.Collections.Specialized;
using System.Linq;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Query;
using Xunit;

namespace Firestorm.Endpoints.Tests.Query
{
    public class QueryStringCollectionQueryTests
    {
        [Fact]
        public void GivenFieldsString_ContainsFields()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration(), "fields=id,test,foo");

            Assert.Contains("id", query.SelectFields);
            Assert.Contains("test", query.SelectFields);
            Assert.Contains("foo", query.SelectFields);
        }

        [Fact]
        public void GivenFieldsStringWithQuertionMark_ContainsFields()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration(), "?fields=id,test,foo");

            Assert.Contains("id", query.SelectFields);
            Assert.Contains("test", query.SelectFields);
            Assert.Contains("foo", query.SelectFields);
        }

        [Fact]
        public void GivenFieldsCollection_ContainsFields()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration(), new NameValueCollection {
                { "fields", "test" },
                { "where", "age>25" },
                });
            
            Assert.Contains("test", query.SelectFields);
            Assert.Contains(query.FilterInstructions, fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.GreaterThan && fi.ValueString == "25");
        }

        [Fact]
        public void GivenFilters_ContainsFilters()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration(), "where=age>=25&where=age<45");

            Assert.Contains(query.FilterInstructions, fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.GreaterThanOrEquals && fi.ValueString == "25");
            Assert.Contains(query.FilterInstructions, fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.LessThan && fi.ValueString == "45");
        }

        [Fact]
        public void SpecialKeysEnabled_GivenSpecialFilterKeys_ContainsFilters()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration
            {
                SpecialFilterKeysEnabled = true
            }, "age>=25&age<45");

            Assert.Contains(query.FilterInstructions, fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.GreaterThanOrEquals && fi.ValueString == "25");
            Assert.Contains(query.FilterInstructions, fi => fi.FieldName == "age" && fi.Operator == FilterComparisonOperator.LessThan && fi.ValueString == "45");
        }

        [Fact]
        public void SpecialKeysDisabled_GivenSpecialFilterKeys_DoesntContainFilters()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration
            {
                SpecialFilterKeysEnabled = false
            }, "age>=25&age<45");

            Assert.Null(query.FilterInstructions);
        }

        [Fact]
        public void GivenSortStringWithComma_ContainsSortInstructions()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration(), "sort=someDate+asc,another+desc");

            Assert.Contains(query.SortInstructions, si => si.FieldName == "someDate" && si.Direction == SortDirection.Ascending);
            Assert.Contains(query.SortInstructions, si => si.FieldName == "another" && si.Direction == SortDirection.Descending);
        }

        [Fact]
        public void GivenSortStrings_ContainsSortInstructions()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration(), "sort=someDate+asc&sort=another+desc");

            Assert.Contains(query.SortInstructions, si => si.FieldName == "someDate" && si.Direction == SortDirection.Ascending);
            Assert.Contains(query.SortInstructions, si => si.FieldName == "another" && si.Direction == SortDirection.Descending);
        }

        [Fact]
        public void GivenNoSpecificSortDirection_ContainsUnspecified()
        {
            var query = new QueryStringCollectionQuery(new QueryStringConfiguration(), "sort=column");

            Assert.Contains(query.SortInstructions, si => si.FieldName == "column" && si.Direction == SortDirection.Unspecified);
        }
    }
}
