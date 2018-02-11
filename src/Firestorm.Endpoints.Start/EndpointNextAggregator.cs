using System;

namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// Aggregates calls to <see cref="IRestEndpoint.Next"/> based on the full resource path.
    /// </summary>
    public class EndpointNextAggregator
    {
        private readonly IRestEndpoint _startEndpoint;

        public EndpointNextAggregator(IRestEndpoint startEndpoint)
        {
            _startEndpoint = startEndpoint;
        }

        public IRestEndpoint AggregateNext(string fullResourcePath)
        {
            if (string.IsNullOrEmpty(fullResourcePath))
                return _startEndpoint;

            string[] dirs = fullResourcePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            IRestEndpoint endpoint = _startEndpoint;

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
                : base("Error loading the '" + dir + "' endpoint: " + innerException.Message, innerException)
            { }
        }
    }
}