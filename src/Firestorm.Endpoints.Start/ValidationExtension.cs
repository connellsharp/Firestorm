namespace Firestorm.Endpoints.Start
{
    public static class ValidationExtension
    {
        public static void EnsureValid(this FirestormConfiguration configuration)
        {
            if (configuration.StartResourceFactory == null)
                throw new FirestormConfigurationException("StartResourceFactory cannot be null.");

            if (configuration.EndpointConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration cannot be null.");

            if (configuration.EndpointConfiguration.QueryStringConfiguration == null)
                throw new FirestormConfigurationException("EndpointConfiguration.QueryStringConfiguration cannot be null.");

            if (configuration.EndpointConfiguration.RequestStrategies == null)
                throw new FirestormConfigurationException("EndpointConfiguration.RequestStrategies cannot be null.");

            if (configuration.EndpointConfiguration.ResponseContentGenerator == null)
                throw new FirestormConfigurationException("EndpointConfiguration.ResponseContentGenerator cannot be null.");
        }
    }
}