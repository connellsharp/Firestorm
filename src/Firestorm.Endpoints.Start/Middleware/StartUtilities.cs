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
                try
                {
                    endpoint = endpoint.Next(dir);
                }
                catch (Exception ex)
                {
                    throw new NextEndpointErrorException(dir, ex);
                }

                if (endpoint == null)
                    throw new NextEndpointNotFoundException(dir);
            }

            return endpoint;
        }

        private class NextEndpointNotFoundException : RestApiException
        {
            public NextEndpointNotFoundException(string dir)
                : base(ErrorStatus.NotFound, "The '" + dir + "' endpoint was not found.")
            { }
        }

        private class NextEndpointErrorException : RestApiException
        {
            public NextEndpointErrorException(string dir, Exception innerException)
                : base("There was an error loading the '" + dir + "' endpoint.", innerException)
            { }
        }
    }
}