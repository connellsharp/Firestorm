using System.Collections.Generic;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.Pagination;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Strategies;

namespace Firestorm.Endpoints
{
    public interface IEndpointApplicationFactory
    {
        EndpointApplication CreateApplication();
    }

    internal class EndpointApplicationFactory : IEndpointApplicationFactory
    {
        private readonly EndpointConfiguration _configuration;

        public EndpointApplicationFactory(EndpointConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EndpointApplication CreateApplication()
        {
            return new EndpointApplication
            {
                Modifiers = new DefaultResponseModifiers(_configuration.Response),
                QueryCreator = new DefaultQueryCreator(_configuration.QueryString),
                ExecutorFactory = new DefaultExecutorFactory(_configuration),
                NameSwitcher = new DefaultNamingConventionSwitcher(),
                PageLinkCalculator = new PageLinkCalculator(_configuration.Response.Pagination),
                Strategies = new CommandStrategySets(),
                EndpointResolver = new EndpointResolver()
            };
        }
    }
}