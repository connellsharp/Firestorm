namespace Firestorm.Endpoints.Start
{
    public static class StartEndpointUtility
    {
        public static IRestEndpoint GetEndpointFromPath(FirestormConfiguration configuration, IRestEndpointContext endpointContext, string resourcePath)
        {
            return GetEndpointFromPath(configuration.StartResourceFactory, endpointContext, resourcePath);
        }

        public static IRestEndpoint GetEndpointFromPath(IStartResourceFactory startResourceFactory, IRestEndpointContext endpointContext, string resourcePath)
        {
            IRestResource startResource = startResourceFactory.GetStartResource(endpointContext);
            IRestEndpoint startEndpoint = Endpoint.GetFromResource(endpointContext, startResource);

            var nextAggregator = new EndpointNextAggregator(startEndpoint);
            IRestEndpoint endpoint = nextAggregator.AggregateNext(resourcePath);

            // TODO naming convention switching here?

            return endpoint;
        }
    }
}