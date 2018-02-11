using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.Tests.Unit.Endpoints.Stubs;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Start
{
    public class FirestormMiddlewareHelperCollectionTests
    {
        private readonly MockStartResource _startResource;
        private readonly FirestormConfiguration _firestormConfiguration;

        public FirestormMiddlewareHelperCollectionTests()
        {
            _startResource = new MockStartResource();

            _firestormConfiguration = new FirestormConfiguration
            {
                StartResourceFactory = new SingletonStartResourceFactory(_startResource)
            };
        }

        [Fact]
        public async Task Middleware_Get_ReturnsCollectionBody()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "GET",
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(HttpStatusCode.OK, handler.ResponseStatusCode);
            // TODO test the response is the collection
            //var enumerable = Assert.IsAssignableFrom<IEnumerable>(handler.ResponseBody);
            //Assert.Equal(_startResource.List.Count, enumerable.OfType<object>().Count());
        }

        [Fact]
        public async Task Middleware_Post_AddsToList()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "POST",
                RequestContentReader = new MockJsonReader(@"{ value: ""New value"" }"),
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            int startCount = _startResource.List.Count;

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(startCount + 1, _startResource.List.Count);
            Assert.Equal(HttpStatusCode.Created, handler.ResponseStatusCode);
        }

        [Fact]
        public async Task Middleware_Put_405()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "PUT",
                RequestContentReader = new MockJsonReader(@"New value"),
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(HttpStatusCode.MethodNotAllowed, handler.ResponseStatusCode);
        }

        [Fact]
        public async Task Middleware_Delete_405()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "DELETE",
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(HttpStatusCode.MethodNotAllowed, handler.ResponseStatusCode);
        }

        [Fact]
        public async Task Middleware_PostSalarBody_400()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "POST",
                RequestContentReader = new MockJsonReader(@"New value"),
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(HttpStatusCode.BadRequest, handler.ResponseStatusCode);
        }

        [Fact]
        public async Task Middleware_Options_Dunno()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "OPTIONS",
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

        }

        [Fact]
        public async Task Middleware_InvalidMethod_405()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "NOTMETHOD",
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(HttpStatusCode.MethodNotAllowed, handler.ResponseStatusCode);
        }

        public class MockStartResource : IRestCollection
        {
            public List<string> List = new List<string>
            {
                "Zero",
                "One",
                "Two",
                "Three"
            };

            public async Task<RestCollectionData> QueryDataAsync(IRestCollectionQuery query)
            {
                if (query.FilterInstructions != null && query.FilterInstructions.Any())
                    throw new NotImplementedException("Not implemented filtering in mock collection.");

                if (query.SortInstructions != null && query.SortInstructions.Any())
                    throw new NotImplementedException("Not implemented sorting in mock collection.");

                if (query.SelectFields != null && query.SelectFields.Any())
                    throw new NotImplementedException("Not implemented selecting in mock collection.");

                IEnumerable<RestItemData> items = List.Select((s, i) => new RestItemData(new
                {
                    index = i,
                    value = s
                }));

                return new RestCollectionData(items, null);
            }

            public IRestItem GetItem(string identifier, string identifierName = null)
            {
                int index = int.Parse(identifier);
                return new MockRestStringItem(index, List[index]);
            }

            public IRestDictionary ToDictionary(string identifierName)
            {
                throw new NotImplementedException();
            }

            public async Task<CreatedItemAcknowledgment> AddAsync(RestItemData itemData)
            {
                List.Add((string) itemData["value"]);
                return new CreatedItemAcknowledgment(123);
            }
        }

        public class MockRestStringItem : IRestItem
        {
            private readonly int _index;
            private readonly string _value;

            public MockRestStringItem(int index, string value)
            {
                _index = index;
                _value = value;
            }

            public IRestResource GetField(string fieldName)
            {
                throw new NotImplementedException();
            }

            public async Task<RestItemData> GetDataAsync(IEnumerable<string> fieldNames)
            {
                return new RestItemData(new
                {
                    index = _index,
                    value = _value
                });
            }

            public Task<Acknowledgment> EditAsync(RestItemData itemData)
            {
                throw new NotImplementedException();
            }

            public Task<Acknowledgment> DeleteAsync()
            {
                throw new NotImplementedException();
            }
        }
    }

    internal class MockJsonReader : IContentReader
    {
        private readonly string _json;

        public MockJsonReader(string json)
        {
            _json = json;
        }

        public Stream GetContentStream()
        {
            return new MemoryStream(Encoding.Default.GetBytes(_json));
        }

        public string GetMimeType()
        {
            return "application/json";
        }
    }
}