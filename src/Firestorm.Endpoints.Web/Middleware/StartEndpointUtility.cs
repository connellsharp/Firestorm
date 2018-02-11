using Firestorm.Endpoints.Naming;
using Firestorm.Endpoints.Start;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Web
{
    public static class StartEndpointUtility
    {
        public static IRestEndpoint GetEndpointFromPath(FirestormConfiguration configuration, IRestEndpointContext endpointContext, string resourcePath)
        {
            INamingConventionSwitcher switcher = configuration.EndpointConfiguration.NamingConventionSwitcher ?? new VoidNamingConventionSwitcher();
            return GetEndpointFromPath(configuration.StartResourceFactory, endpointContext, resourcePath, switcher);
        }

        public static IRestEndpoint GetEndpointFromPath(IStartResourceFactory startResourceFactory, IRestEndpointContext endpointContext, string resourcePath)
        {
            return GetEndpointFromPath(startResourceFactory, endpointContext, resourcePath, new VoidNamingConventionSwitcher());
        }

        public static IRestEndpoint GetEndpointFromPath(IStartResourceFactory startResourceFactory, IRestEndpointContext endpointContext, string resourcePath, [NotNull] INamingConventionSwitcher namingConventionSwitcher)
        {
            IRestResource startResource = startResourceFactory.GetStartResource(endpointContext);
            IRestEndpoint startEndpoint = Endpoint.GetFromResource(endpointContext, startResource);

            var nextAggregator = new EndpointNextAggregator(startEndpoint, namingConventionSwitcher);
            IRestEndpoint endpoint = nextAggregator.AggregateNext(resourcePath);

            var wrappedEndpoint = new NamingSwitcherEndpointWrapper(endpoint, namingConventionSwitcher);
            return wrappedEndpoint;
        }
    }
}