using Firestorm.Endpoints.Configuration;
using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    public static class ValidationExtension
    {   
        public static void EnsureValid([NotNull] this EndpointConfiguration configuration)
        {
            if (configuration == null)
                throw new FirestormConfigurationException("EndpointConfiguration cannot be null.");

            if (configuration.QueryString == null)
                throw new FirestormConfigurationException("EndpointConfiguration.QueryStringConfiguration cannot be null.");

            if (configuration.Response == null)
                throw new FirestormConfigurationException("EndpointConfiguration.ResponseConfiguration cannot be null.");

            if (configuration.Response.Pagination == null)
                throw new FirestormConfigurationException("EndpointConfiguration.PageConfiguration cannot be null.");
        }
    }
}