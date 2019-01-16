using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Tests.Stubs;
using Xunit;

namespace Firestorm.Endpoints.Tests.Start
{
    public class ValidationExtensionTests
    {
        [Fact]
        public void EnsureValid_DefaultConfiguration_DoesntThrow()
        {
            var config = new DefaultEndpointConfiguration();

            config.EnsureValid();
        }

        [Fact]
        public void EnsureValid_NullQueryStringConfiguration_Throws()
        {
            var config = new EndpointConfiguration
            {
                QueryString = null
            };

            Action ensureValid = () => config.EnsureValid();

            Assert.Throws<FirestormConfigurationException>(ensureValid);
        }

        [Fact]
        public void EnsureValid_NullRequestStrategies_Throws()
        {
            var config = new EndpointConfiguration
            {
                Strategies = null
            };

            Action ensureValid = () => config.EnsureValid();

            Assert.Throws<FirestormConfigurationException>(ensureValid);
        }

        [Fact]
        public void EnsureValid_NullResponseConfiguration_Throws()
        {
            var config = new EndpointConfiguration
            {
                Response = null
            };

            Action ensureValid = () => config.EnsureValid();

            Assert.Throws<FirestormConfigurationException>(ensureValid);
        }
    }
}
