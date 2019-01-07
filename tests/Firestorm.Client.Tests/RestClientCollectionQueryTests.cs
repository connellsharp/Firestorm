using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Client;
using Xunit;

namespace Firestorm.Client.Tests
{
    public class RestClientCollectionQueryTests
    {
        private const string BASE_PATH = "over/here/yeah/";

        private readonly MockHttpClientCreator _clientCreator;
        private readonly RestClientCollection _collection;
        
        public RestClientCollectionQueryTests()
        {
            _clientCreator = new MockHttpClientCreator();
            _collection = new RestClientCollection(_clientCreator, BASE_PATH);
        }

        [Fact]
        public async Task QueryData_RequestWithField_IncludedInString()
        {
            _clientCreator.ResponseBody = "[ { testFieldHere: 12345 } ]";

            await _collection.QueryDataAsync(new TestCollectionQuery
            {
                SelectFields = new[] { "testFieldHere" }
            });

            Assert.True(_clientCreator.LastRequest.RequestUri.ToString().Contains("testFieldHere"));
        }

        [Fact]
        public async Task QueryData_RequestWithMultiFields_IncludedCommaSeparatedInString()
        {
            _clientCreator.ResponseBody = "[ { testFieldHere: 12345, anotherField: 123456 } ]";

            await _collection.QueryDataAsync(new TestCollectionQuery
            {
                SelectFields = new[] { "testFieldHere", "anotherField" }
            });

            string decoded = Uri.UnescapeDataString(_clientCreator.LastRequest.RequestUri.Query);
            Assert.True(decoded.Contains("testFieldHere,anotherField"));
        }

        [Fact]
        public async Task QueryData_RequestWithFieldContainingComma_Throws()
        {
            _clientCreator.ResponseBody = "[ { testFieldHere: 12345, anotherField: 123456 } ]";

            await Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _collection.QueryDataAsync(new TestCollectionQuery
                {
                    SelectFields = new[] { "testFieldHere,anotherField" }
                });
            });
        }

        [Fact]
        public async Task QueryData_RequestWithEqualsFilter_IncludedInString()
        {
            _clientCreator.ResponseBody = "[]";

            await _collection.QueryDataAsync(new TestCollectionQuery
            {
                FilterInstructions = new List<FilterInstruction>
                {
                    new FilterInstruction("thisField", FilterComparisonOperator.Equals, "shouldEqualThis")
                }
            });

            Assert.True(_clientCreator.LastRequest.RequestUri.ToString().Contains("thisField=shouldEqualThis"));
        }

        [Fact]
        public async Task QueryData_RequestWithGreaterThanFilter_IncludedInString()
        {
            _clientCreator.ResponseBody = "[]";

            await _collection.QueryDataAsync(new TestCollectionQuery
            {
                FilterInstructions = new List<FilterInstruction>
                {
                    new FilterInstruction("numberField", FilterComparisonOperator.GreaterThan, "42")
                }
            });

            Assert.True(_clientCreator.LastRequest.RequestUri.ToString().Contains("numberField>42"));
        }

        [Fact]
        public async Task QueryData_RequestWithMultiEquals_BothIncludedInString()
        {
            _clientCreator.ResponseBody = "[]";

            await _collection.QueryDataAsync(new TestCollectionQuery
            {
                FilterInstructions = new List<FilterInstruction>
                {
                    new FilterInstruction("thisField", FilterComparisonOperator.Equals, "shouldEqualThis"),
                    new FilterInstruction("numberField", FilterComparisonOperator.Equals, "5")
                }
            });

            Assert.True(_clientCreator.LastRequest.RequestUri.ToString().Contains("thisField=shouldEqualThis"));
            Assert.True(_clientCreator.LastRequest.RequestUri.ToString().Contains("numberField=5"));
        }

        [Fact]
        public async Task QueryData_ParsedResponse_Correct()
        {
            _clientCreator.ResponseBody = "[ { id: 123 } ]";
            RestCollectionData queriedData = await _collection.QueryDataAsync(new TestCollectionQuery());

            Assert.Equal(1, queriedData.Items.Count());
            Assert.Equal(123, Convert.ToInt32(queriedData.Items.First()["Id"]));
        }
    }
}
