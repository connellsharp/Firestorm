using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.Pagination;
using Firestorm.Endpoints.Responses;
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

        public void AddTo(EndpointServices services)
        {
            services.Modifiers = new DefaultResponseModifiers(_configuration.Response);
            services.QueryCreator = new DefaultQueryCreator(_configuration.QueryString);
            services.ExecutorFactory = new DefaultExecutorFactory(_configuration);
            services.NameSwitcher = new DefaultNamingConventionSwitcher();
            services.PageLinkCalculator = new PageLinkCalculator(_configuration.Response.Pagination);
            services.Strategies = new CommandStrategySets();
            services.EndpointResolver = new EndpointResolver();
            services.UrlHelper = new DefaultUrlHelper(_configuration.Url);
        }
    }
}