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
            
            configuration.EndpointConfiguration.EnsureValid();
        }
        
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