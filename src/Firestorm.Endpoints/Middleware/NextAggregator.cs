using System;
using Firestorm.Endpoints.Configuration;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Web
{
    /// <summary>
    /// Aggregates calls to <see cref="IRestEndpoint.Next"/> based on the full resource path.
    /// </summary>
    internal class NextAggregator
    {
        private readonly IRestEndpoint _startEndpoint;
        private readonly INamingConventionSwitcher _namingConventionSwitcher;

        public NextAggregator([NotNull] IRestEndpoint startEndpoint, [NotNull] INamingConventionSwitcher namingConventionSwitcher)
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
                var path = new AggregatorNextPath(dir, _namingConventionSwitcher);

                try
                {
                    endpoint = endpoint.Next(path);
                }
                catch (Exception ex)
                {
                    throw new NextEndpointErrorException(path, ex);
                }

                if (endpoint == null)
                {
                    throw new NextEndpointNotFoundException(path);
                }
            }

            return endpoint;
        }

        private class NextEndpointNotFoundException : RestApiException
        {
            public NextEndpointNotFoundException(AggregatorNextPath dir)
                : base(ErrorStatus.NotFound, "The '" + dir.Raw + "' endpoint was not found.")
            { }
        }

        private class NextEndpointErrorException : RestApiException
        {
            public NextEndpointErrorException(AggregatorNextPath dir, Exception innerException)
                : base("Error loading the '" + dir.Raw + "' endpoint: " + innerException.Message, innerException)
            { }
        }
    }
}