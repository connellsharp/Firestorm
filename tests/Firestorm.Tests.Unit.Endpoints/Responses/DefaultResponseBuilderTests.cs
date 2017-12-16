using System.Linq;
using Firestorm.Endpoints.Responses;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Responses
{
    public class DefaultResponseBuilderTests
    {
        public DefaultResponseBuilderTests()
        {
        }

        [Fact]
        public void DefaultResponseBuilders_StatusFieldSuccessBoolean_AddsBuilder()
        {
            var builder = new DefaultResponseBuilders(new ResponseConfiguruation
            {
                StatusField = ResponseStatusField.SuccessBoolean
            });
            
            bool hasBuilder = builder.Any(b => b is SuccessBooleanResponseBuilder);
            Assert.True(hasBuilder);
        }

        [Fact]
        public void DefaultResponseBuilders_StatusFieldStatusCode_AddsBuilder()
        {
            var builder = new DefaultResponseBuilders(new ResponseConfiguruation
            {
                StatusField = ResponseStatusField.StatusCode
            });

            bool hasBuilder = builder.Any(b => b is StatusCodeResponseBuilder);
            Assert.True(hasBuilder);
        }
    }
}