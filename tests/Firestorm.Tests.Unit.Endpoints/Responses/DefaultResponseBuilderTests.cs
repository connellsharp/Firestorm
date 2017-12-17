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
        public void StatusField_SuccessBoolean_AddsBuilder()
        {
            var builder = new DefaultResponseBuilders(new ResponseConfiguruation
            {
                StatusField = ResponseStatusField.SuccessBoolean
            });
            
            bool hasBuilder = builder.Any(b => b is SuccessBooleanResponseBuilder);
            Assert.True(hasBuilder);
        }

        [Fact]
        public void StatusField_StatusCode_AddsBuilder()
        {
            var builder = new DefaultResponseBuilders(new ResponseConfiguruation
            {
                StatusField = ResponseStatusField.StatusCode
            });

            bool hasBuilder = builder.Any(b => b is StatusCodeResponseBuilder);
            Assert.True(hasBuilder);
        }

        [Fact]
        public void WrapCollectionResponseBody_AddsPagedBodyBuilder()
        {
            var builder = new DefaultResponseBuilders(new ResponseConfiguruation
            {
                PageConfiguration =
                {
                    WrapCollectionResponseBody = true
                }
            });

            bool hasBuilder = builder.Any(b => b is PagedBodyResponseBuilder);
            Assert.True(hasBuilder);
        }

        [Fact]
        public void ShowDeveloperErrors_AddsDeveloperExceptionBuilder()
        {
            var builder = new DefaultResponseBuilders(new ResponseConfiguruation
            {
                ShowDeveloperErrors = true
            });

            bool hasBuilder = builder.Any(b => b is DeveloperExceptionInfoResponseBuilder);
            Assert.True(hasBuilder);
        }
    }
}