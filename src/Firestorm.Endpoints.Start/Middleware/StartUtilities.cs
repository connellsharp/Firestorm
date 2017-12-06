using System;

namespace Firestorm.Endpoints.Start
{
    public static class StartUtilities
    {
        public static IRestEndpoint GetEndpointFromPath(IStartResourceFactory starter, IRestEndpointContext endpointContext, string fullResourcePath)
        {
            IRestResource startResource = starter.GetStartResource(endpointContext);
            IRestEndpoint endpoint = Endpoint.GetFromResource(endpointContext, startResource);

            endpoint = AggregateNextCalls(endpoint, fullResourcePath);

            return endpoint;
        }

        private static IRestEndpoint AggregateNextCalls(IRestEndpoint endpoint, string fullResourcePath)
        {
            if (string.IsNullOrEmpty(fullResourcePath))
                return endpoint;

            string[] dirs = fullResourcePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string dir in dirs)
            {
                endpoint = endpoint.Next(dir);
                // TODO null 404?
            }

            return endpoint;
        }
    }
}