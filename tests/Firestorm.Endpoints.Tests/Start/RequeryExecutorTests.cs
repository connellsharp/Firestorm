using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Endpoints.Responses;
using Firestorm.Rest.Web;
using Firestorm.Testing;
using Moq;
using Xunit;

namespace Firestorm.Endpoints.Tests.Start
{
    public class RequeryExecutorTests
    {
        private readonly Fixture _fixture;

        public RequeryExecutorTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Theory]
        [InlineData("PUT")]
        [InlineData("POST")]
        [InlineData("PATCH")]
        public async Task KnownMethod_Execute_AddsResourceToBody(string method)
        {
            _fixture.Inject<Feedback>(new AcknowledgmentFeedback(null));

            var body = new ItemBody(new RestItemData());
            _fixture.Inject<ResourceBody>(body);

            var executor = _fixture.Create<RequeryExecutor>();

            var readerMock = new Mock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns(method);

            var mockModifier = new Mock<IResponseModifier>();
            var response = new Response("test");
            var responseBuilder = new ResponseBuilder(response, mockModifier.Object);

            await executor.ExecuteAsync(readerMock.Object, responseBuilder);

            mockModifier.Verify(m => m.AddResource(response, body));
        }

        [Theory]
        [InlineData("DELETE")]
        [InlineData("DESTROY")]
        public async Task UnknownMethod_Execute_DoesNotAddBody(string method)
        {
            _fixture.Inject<Feedback>(new AcknowledgmentFeedback(null));
            _fixture.Inject<ResourceBody>(new ItemBody(new RestItemData()));

            var executor = _fixture.Create<RequeryExecutor>();

            var readerMock = new Mock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns(method);

            var mockModifier = new Mock<IResponseModifier>();
            var response = new Response("test");
            var responseBuilder = new ResponseBuilder(response, mockModifier.Object);

            await executor.ExecuteAsync(readerMock.Object, responseBuilder);

            mockModifier.Verify(m => m.AddResource(response, It.IsAny<ResourceBody>()), Times.Never);
        }

        [Fact]
        public async Task CreatedItemAck_Execute_NewItemIsAddedToResponse()
        {
            var feedback = new AcknowledgmentFeedback(new CreatedItemAcknowledgment(123));
            _fixture.Inject<Feedback>(feedback);
            _fixture.Inject<ResourceBody>(new ItemBody(new RestItemData()));

            var endpointMock = _fixture.FreezeMock<IRestEndpoint>();
            
            _fixture.FreezeMock<IExecutor>()
                .Setup(e => e.ExecuteAsync(It.IsAny<IRequestReader>(), It.IsAny<ResponseBuilder>()))
                .ReturnsAsync(feedback);

            var executor = _fixture.Create<RequeryExecutor>();

            var readerMock = new Mock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns("POST");

            var mockModifier = new Mock<IResponseModifier>();
            var response = new Response("test");
            var responseBuilder = new ResponseBuilder(response, mockModifier.Object);

            await executor.ExecuteAsync(readerMock.Object, responseBuilder);
            
            endpointMock.Verify(e => e.Next(It.IsAny<INextPath>()));
        }
    }
}