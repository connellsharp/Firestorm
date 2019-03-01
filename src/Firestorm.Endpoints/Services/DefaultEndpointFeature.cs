using Firestorm.Endpoints.Executors;
using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.QueryCreators;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Responses.Pagination;
using Firestorm.Endpoints.Strategies;
using Firestorm.Features;

namespace Firestorm.Endpoints
{
    internal class DefaultEndpointFeature : IFeature<EndpointServices>
    {
        private readonly EndpointConfiguration _configuration;

        public DefaultEndpointFeature(EndpointConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EndpointServices Apply(EndpointServices services)
        {
            services.Modifiers = new DefaultResponseModifiers(_configuration.Response);
            services.QueryCreator = new DefaultQueryCreator(_configuration.QueryString);
            services.ExecutorFactory = new DefaultExecutorFactory(_configuration);
            services.NameSwitcher = new DefaultNamingConventionSwitcher(_configuration.NamingConventions);
            services.PageLinkCalculator = new PageLinkCalculator(_configuration.Response.Pagination);
            services.Strategies = new CommandStrategySets();
            services.EndpointResolver = new EndpointResolver();
            services.UrlHelper = new DefaultUrlHelper(_configuration.Url);

            return services;
        }
    }
}