using System.Net;
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
    public class PreconditionsExecutorTests
    {
        private readonly Fixture _fixture;

        public PreconditionsExecutorTests()
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
        public async Task Invoke_FailsPreconditions_ExpectedStatusCode(string method, HttpStatusCode expectedStatusCode)
        {
            _fixture.FreezeMock<IRestEndpoint>(m =>
            {
                m.SetupIgnoreArgs(a => a.EvaluatePreconditions(null)).Returns(false);
                m.SetupIgnoreArgs(a => a.GetAsync(null)).ReturnsAsync(new ItemBody(null));
            });

            var readerMock = new Mock<IRequestReader>();
            readerMock.SetupGet(a => a.RequestMethod).Returns(method);
            
            var response = new Response("test");
            var responseBuilder = new ResponseBuilder(response, new ErrorResponseModifier());
            
            var executor = _fixture.Create<PreconditionsExecutor>();
            await executor.ExecuteAsync(readerMock.Object, responseBuilder);
            
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}