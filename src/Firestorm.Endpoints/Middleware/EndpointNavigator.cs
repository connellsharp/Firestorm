using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    public class EndpointNavigator
    {
        private readonly IRequestContext _requestContext;
        private readonly IStartResourceFactory _startResourceFactory;
        private readonly IEndpointCoreServices _services;
        
        public EndpointNavigator(IRequestContext requestContext, IStartResourceFactory startResourceFactory, IEndpointCoreServices services)
        {
            _requestContext = requestContext;
            _startResourceFactory = startResourceFactory;
            _services = services;
        }

        public IRestEndpoint GetEndpointFromPath(string resourcePath)
        {
            IRestResource startResource = _startResourceFactory.GetStartResource(_requestContext);

            var context = new EndpointContext(_requestContext, _services);
            IRestEndpoint startEndpoint = _services.EndpointResolver.GetFromResource(context, startResource);

            var nextAggregator = new NextAggregator(startEndpoint, _services.NameSwitcher ?? new VoidNamingConventionSwitcher());
            IRestEndpoint endpoint = nextAggregator.AggregateNext(resourcePath);

            //var wrappedEndpoint = new NamingSwitcherEndpointWrapper(endpoint, namingConventionSwitcher);

            return endpoint;
        }
    }
}