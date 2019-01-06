using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Rest.Web;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Web;
using Moq;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Start
{
    public class EndpointInvokerTests
    {
        private readonly Fixture _fixture;

        public EndpointInvokerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Theory]
        [InlineData("GET", HttpStatusCode.NotModified)]
        [InlineData("PUT", HttpStatusCode.PreconditionFailed)]
        [InlineData("POST", HttpStatusCode.PreconditionFailed)]
        [InlineData("DELETE", HttpStatusCode.PreconditionFailed)]
        [InlineData("PATCH", HttpStatusCode.PreconditionFailed)]
        [InlineData("DESTROY", HttpStatusCode.MethodNotAllowed)]
        public async Task Invoke_FailsPreconditions_ExpecedStatusCode(string method, HttpStatusCode expectedStatusCode)
        {
            _fixture.FreezeMock<IRestEndpoint>(m =>
            {
                m.SetupIgnoreArgs(a => a.EvaluatePreconditions(null)).Returns(false);
            });

            var readerMock = _fixture.FreezeMock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns(method);
            
            var response = new Response("test");
            _fixture.Inject(new ResponseBuilder(response, new PaginationHeadersResponseModifier()));

            var invoker = _fixture.Create<EndpointExecutor>();
            await invoker.ExecuteAsync();
            
            Assert.Equal(expectedStatusCode, response.StatusCode);
            //handlerMock.Verify(a => a.SetStatusCode(expectedStatusCode));
        }

        [Theory]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, true, true)]
        [InlineData(false, false, false)]
        public async Task InvokeGet_BodyWithNextPath_AddsLinkHeader(bool hasNextPath, bool hasPrevPath, bool shouldHaveLinkHeader)
        {
            _fixture.FreezeMock<IRestEndpoint>(m =>
            {
                m.SetupIgnoreArgs(a => a.EvaluatePreconditions(null)).Returns(true);

                var pageLinks = new PageLinks
                {
                    Next = hasNextPath ? new PageInstruction { PageNumber = 3 } : null,
                    Previous = hasPrevPath ? new PageInstruction { PageNumber = 1 } : null,
                };

                m.Setup(a => a.GetAsync(It.IsAny<IRestCollectionQuery>())).ReturnsAsync(new CollectionBody(null, pageLinks));
            });

            var readerMock = _fixture.FreezeMock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns("GET");

            var response = new Response("test");
            _fixture.Inject(new ResponseBuilder(response, new PaginationHeadersResponseModifier()));

            var invoker = _fixture.Create<EndpointExecutor>();
            await invoker.ExecuteAsync();
            
            Assert.Equal(shouldHaveLinkHeader, response.Headers.ContainsKey("Link"));
            //handlerMock.Verify(a => a.SetResponseHeader("Link", It.IsAny<string>()), Times.Exactly(shouldHaveLinkHeader ? 1 : 0));
        }
    }
}
