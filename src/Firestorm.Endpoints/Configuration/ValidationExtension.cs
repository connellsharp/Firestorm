using Firestorm.Endpoints.Configuration;
using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    public static class ValidationExtension
    {   
        public static void EnsureValid([NotNull] this RestEndpointConfiguration configuration)
        {
            if (configuration == null)
                throw new FirestormConfigurationException("EndpointConfiguration cannot be null.");

            if (configuration.QueryStringConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration.QueryStringConfiguration cannot be null.");

            if (configuration.RequestStrategies == null)
                throw new FirestormConfigurationException("EndpointConfiguration.RequestStrategies cannot be null.");

            if (configuration.ResponseConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration.ResponseConfiguration cannot be null.");

            if (configuration.ResponseConfiguration.PageConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration.PageConfiguration cannot be null.");
        }
    }
}