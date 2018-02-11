using System;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// Aggregates calls to <see cref="IRestEndpoint.Next"/> based on the full resource path.
    /// </summary>
    public class EndpointNextAggregator
    {
        private readonly IRestEndpoint _startEndpoint;
        private readonly INamingConventionSwitcher _namingConventionSwitcher;

        public EndpointNextAggregator([NotNull] IRestEndpoint startEndpoint, [NotNull] INamingConventionSwitcher namingConventionSwitcher)
        {
            _startEndpoint = startEndpoint ?? throw new ArgumentNullException(nameof(startEndpoint));
            _namingConventionSwitcher = namingConventionSwitcher ?? throw new ArgumentNullException(nameof(namingConventionSwitcher));
        }

        public IRestEndpoint AggregateNext(string fullResourcePath)
        {
            if (string.IsNullOrEmpty(fullResourcePath))
                return _startEndpoint;

            IRestEndpoint endpoint = _startEndpoint;
            string[] dirs = fullResourcePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string dir in dirs)
            {
                string directory = _namingConventionSwitcher.ConvertRequestedToCoded(dir);

                try
                {
                    endpoint = endpoint.Next(directory);
                }
                catch (Exception ex)
                {
                    throw new NextEndpointErrorException(directory, ex);
                }

                if (endpoint == null)
                {
                    throw new NextEndpointNotFoundException(directory);
                }
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