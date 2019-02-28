using System.Net;
using System.Threading.Tasks;
using Firestorm.Endpoints.Tests.Start.Readers;
using Firestorm.Endpoints.Tests.Stubs;
using Xunit;

namespace Firestorm.Endpoints.Tests.Start
{
    public class EndpointsRequestInvokerScalarTests
    {
        private readonly FakeScalar _scalar;
        private readonly EndpointsRequestInvoker _invoker;

        public EndpointsRequestInvokerScalarTests()
        {
            _scalar = new FakeScalar();

            var startResourceFactory = new SingletonStartResourceFactory(_scalar);
            _invoker = new EndpointsRequestInvoker(startResourceFactory, new TestEndpointServices());
        }

        [Fact]
        public async Task Invoker_Get_ReturnsStartResourceBody()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "GET",
                ResourcePath = ""
            };

            await _invoker.InvokeAsync(handler, handler, new TestRequestContext());

            Assert.Equal(HttpStatusCode.OK, handler.ResponseStatusCode);
            // TODO too big-a-test. Maybe test invoker alone?
            //Assert.Equal(_startResource.ObjectValue, handler.ResponseBody);
        }

        [Fact]
        public async Task Invoker_Put_EditsScalar()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "PUT",
                RequestContentReader = new MockJsonReader(@"""New value"""),
                ResourcePath = ""
            };

            await _invoker.InvokeAsync(handler, handler, new TestRequestContext());

            Assert.Equal("New value", _scalar.ObjectValue);
        }

        [Fact]
        public async Task Invoker_Delete_SetsToNull()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "DELETE",
                ResourcePath = ""
            };

            await _invoker.InvokeAsync(handler, handler, new TestRequestContext());

            Assert.Equal(null, _scalar.ObjectValue);
        }

        [Fact]
        public async Task Invoker_Options_200Ok()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "OPTIONS",
                ResourcePath = ""
            };

            await _invoker.InvokeAsync(handler, handler, new TestRequestContext());

            Assert.Equal(HttpStatusCode.OK, handler.ResponseStatusCode);
        }

        [Fact]
        public async Task Invoker_InvalidMethod_405()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "NOTMETHOD",
                ResourcePath = ""
            };

            await _invoker.InvokeAsync(handler, handler, new TestRequestContext());

            Assert.Equal(HttpStatusCode.MethodNotAllowed, handler.ResponseStatusCode);
        }

        public class FakeScalar : IRestScalar
        {
            public object ObjectValue { get; set; } = "My value";

            public async Task<object> GetAsync()
            {
                return ObjectValue;
            }

            public async Task<Acknowledgment> EditAsync(object value)
            {
                ObjectValue = value;
                return new Acknowledgment();
            }
        }
    }
}