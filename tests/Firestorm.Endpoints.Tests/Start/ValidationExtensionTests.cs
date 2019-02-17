using System;
using Firestorm.Endpoints.Configuration;
using Xunit;

namespace Firestorm.Endpoints.Tests.Start
{
    public class ValidationExtensionTests
    {
        [Fact]
        public void EnsureValid_DefaultConfiguration_DoesntThrow()
        {
            var config = new EndpointConfiguration();

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
        public void EnsureValid_NullUrlConfiguration_Throws()
        {
            var config = new EndpointConfiguration
            {
                Url = null
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

        [Fact]
        public void EnsureValid_NullNamingConventionConfiguration_Throws()
        {
            var config = new EndpointConfiguration
            {
                NamingConventions = null
            };

            Action ensureValid = () => config.EnsureValid();

            Assert.Throws<FirestormConfigurationException>(ensureValid);
        }
    }
}
