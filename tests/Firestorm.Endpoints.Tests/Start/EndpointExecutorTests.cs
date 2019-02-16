using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Rest.Web;
using Firestorm.Endpoints.Responses;
using Firestorm.Testing;
using Moq;
using Xunit;

namespace Firestorm.Endpoints.Tests.Start
{
    public class EndpointExecutorTests
    {
        private readonly Fixture _fixture;

        public EndpointExecutorTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Fact]
        public async Task Get_Execute_AddsResourceToResponse()
        {
            var body = new ItemBody(new RestItemData());
            
            _fixture.Inject<ResourceBody>(body);
            var executor = _fixture.Create<EndpointExecutor>();

            var readerMock = new Mock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns("GET");

            var mockModifier = new Mock<IResponseModifier>();
            var response = new Response("test");
            var responseBuilder = new ResponseBuilder(response, mockModifier.Object);

            await executor.ExecuteAsync(readerMock.Object, responseBuilder);
            
            mockModifier.Verify(m => m.AddResource(response, body));
        }

        [Fact]
        public async Task PostMethod_Execute_AddsAcknowledgement()
        {
            var acknowledgment = new AcknowledgmentFeedback(null);
            
            _fixture.Inject<Feedback>(acknowledgment);
            var executor = _fixture.Create<EndpointExecutor>();

            var readerMock = new Mock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns("POST");

            var mockModifier = new Mock<IResponseModifier>();
            var response = new Response("test");
            var responseBuilder = new ResponseBuilder(response, mockModifier.Object);

            await executor.ExecuteAsync(readerMock.Object, responseBuilder);
            
            mockModifier.Verify(m => m.AddAcknowledgment(response, acknowledgment.Acknowledgment));
        }

        [Fact]
        public async Task DestroyMethod_Execute_MethodNotAllowed()
        {
            var body = new ItemBody(new RestItemData());
            
            _fixture.Inject<ResourceBody>(body);
            _fixture.Inject<Feedback>(new AcknowledgmentFeedback(null));
            var executor = _fixture.Create<EndpointExecutor>();

            var readerMock = new Mock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns("DESTROY");

            var mockModifier = new Mock<IResponseModifier>();
            var response = new Response("test");
            var responseBuilder = new ResponseBuilder(response, mockModifier.Object);

            await executor.ExecuteAsync(readerMock.Object, responseBuilder);
            
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
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
            
            var executor = _fixture.Create<EndpointExecutor>();

            var readerMock = new Mock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns("GET");

            var response = new Response("test");
            var responseBuilder = new ResponseBuilder(response, new PaginationHeadersResponseModifier());
            
            await executor.ExecuteAsync(readerMock.Object, responseBuilder);
            
            Assert.Equal(shouldHaveLinkHeader, response.Headers.ContainsKey("Link"));
            //handlerMock.Verify(a => a.SetResponseHeader("Link", It.IsAny<string>()), Times.Exactly(shouldHaveLinkHeader ? 1 : 0));
        }
    }
}
