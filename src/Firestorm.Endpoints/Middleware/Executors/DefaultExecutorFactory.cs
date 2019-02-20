namespace Firestorm.Endpoints.Executors
{
    internal class DefaultExecutorFactory : IExecutorFactory
    {
        private readonly EndpointConfiguration _configuration;

        public DefaultExecutorFactory(EndpointConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IExecutor GetExecutor(IRestEndpoint endpoint)
        {
            IExecutor executor = new EndpointExecutor(endpoint);
                
            executor = new PreconditionsExecutor(executor);

            if (_configuration.Response.ResourceOnSuccessfulCommand)
                executor = new RequeryExecutor(executor);

            return executor;
        }
    }
}