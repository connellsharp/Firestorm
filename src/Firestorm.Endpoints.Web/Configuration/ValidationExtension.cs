using JetBrains.Annotations;

namespace Firestorm.Endpoints.Web
{
    public static class ValidationExtension
    {
        public static void EnsureValid([NotNull] this FirestormConfiguration configuration)
        {
            if (configuration == null)
                throw new FirestormConfigurationException("FirestormConfiguration cannot be null.");

            if (configuration.StartResourceFactory == null)
                throw new FirestormConfigurationException("StartResourceFactory cannot be null.");

            if (configuration.EndpointConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration cannot be null.");

            if (configuration.EndpointConfiguration.QueryStringConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration.QueryStringConfiguration cannot be null.");

            if (configuration.EndpointConfiguration.RequestStrategies == null)
                throw new FirestormConfigurationException("EndpointConfiguration.RequestStrategies cannot be null.");

            if (configuration.EndpointConfiguration.ResponseConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration.ResponseConfiguration cannot be null.");

            if (configuration.EndpointConfiguration.ResponseConfiguration.PageConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration.PageConfiguration cannot be null.");
        }
    }
}