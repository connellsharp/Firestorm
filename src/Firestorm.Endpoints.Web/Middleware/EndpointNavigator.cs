using Firestorm.Endpoints.Naming;
using Firestorm.Endpoints.Start;
using Firestorm.Host;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Web
{
    public class EndpointNavigator
    {
        private readonly IRequestContext _requestContext;
        private readonly IStartResourceFactory _startResourceFactory;
        private readonly RestEndpointConfiguration _configuration;
        
        public EndpointNavigator(IRequestContext requestContext, IStartResourceFactory startResourceFactory, RestEndpointConfiguration configuration)
        {
            _requestContext = requestContext;
            _startResourceFactory = startResourceFactory;
            _configuration = configuration;
        }

        public IRestEndpoint GetEndpointFromPath(string resourcePath)
        {
            IRestResource startResource = _startResourceFactory.GetStartResource(_requestContext);

            var context = new EndpointContext(_requestContext, _configuration);
            IRestEndpoint startEndpoint = _configuration.Resolver.GetFromResource(context, startResource);

            var nextAggregator = new NextAggregator(startEndpoint, _configuration.NamingConventionSwitcher ?? new VoidNamingConventionSwitcher());
            IRestEndpoint endpoint = nextAggregator.AggregateNext(resourcePath);

            //var wrappedEndpoint = new NamingSwitcherEndpointWrapper(endpoint, namingConventionSwitcher);

            return endpoint;
        }
    }
}