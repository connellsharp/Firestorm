﻿using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;
using Firestorm.Tests.Unit.Endpoints.Stubs;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Start
{
    public class ValidationExtensionTests
    {
        [Fact]
        public void EnsureValid_DefaultConfiguration_DoesntThrow()
        {
            var config = new DefaultRestEndpointConfiguration();

            config.EnsureValid();
        }

        [Fact]
        public void EnsureValid_NullQueryStringConfiguration_Throws()
        {
            var config = new RestEndpointConfiguration
            {
                QueryStringConfiguration = null
            };

            Action ensureValid = () => config.EnsureValid();

            Assert.Throws<FirestormConfigurationException>(ensureValid);
        }

        [Fact]
        public void EnsureValid_NullRequestStrategies_Throws()
        {
            var config = new RestEndpointConfiguration
            {
                RequestStrategies = null
            };

            Action ensureValid = () => config.EnsureValid();

            Assert.Throws<FirestormConfigurationException>(ensureValid);
        }

        [Fact]
        public void EnsureValid_NullResponseConfiguration_Throws()
        {
            var config = new RestEndpointConfiguration
            {
                ResponseConfiguration = null
            }

            Action ensureValid = () => config.EnsureValid();

            Assert.Throws<FirestormConfigurationException>(ensureValid);
        }
    }
}
