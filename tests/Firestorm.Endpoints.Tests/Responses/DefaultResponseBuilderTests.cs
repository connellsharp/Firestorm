using System.Linq;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Responses;
using Xunit;

namespace Firestorm.Endpoints.Tests.Responses
{

    public class DefaultResponseBuilderTests
    {
        public DefaultResponseBuilderTests()
        {
        }

        [Fact]
        public void StatusField_SuccessBoolean_AddsBuilder()
        {
            var builder = new DefaultResponseModifiers(new ResponseConfiguration
            {
                StatusField = ResponseStatusField.SuccessBoolean
            });
            
            bool hasBuilder = builder.Any(b => b is SuccessBooleanResponseModifier);
            Assert.True(hasBuilder);
        }

        [Fact]
        public void StatusField_StatusCode_AddsBuilder()
        {
            var builder = new DefaultResponseModifiers(new ResponseConfiguration
            {
                StatusField = ResponseStatusField.StatusCode
            });

            bool hasBuilder = builder.Any(b => b is StatusCodeResponseModifier);
            Assert.True(hasBuilder);
        }

        [Fact]
        public void WrapCollectionResponseBody_AddsPagedBodyBuilder()
        {
            var builder = new DefaultResponseModifiers(new ResponseConfiguration
            {
                PageConfiguration =
                {
                    WrapCollectionResponseBody = true
                }
            });

            bool hasBuilder = builder.Any(b => b is PagedBodyResponseModifier);
            Assert.True(hasBuilder);
        }

        [Fact]
        public void ShowDeveloperErrors_AddsDeveloperExceptionBuilder()
        {
            var builder = new DefaultResponseModifiers(new ResponseConfiguration
            {
                ShowDeveloperErrors = true
            });

            bool hasBuilder = builder.Any(b => b is DeveloperExceptionInfoResponseModifier);
            Assert.True(hasBuilder);
        }
    }
}