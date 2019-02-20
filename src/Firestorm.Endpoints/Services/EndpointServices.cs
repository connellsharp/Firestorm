using System.Collections.Generic;
using Firestorm.Endpoints.Executors;
using Firestorm.Endpoints.QueryCreators;
using Firestorm.Endpoints.Requests;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// All the services required to run a Firestorm Endpoints application.
    /// </summary>
    public class EndpointServices : IEndpointCoreServices
    {        
        public IQueryCreator QueryCreator { get; set; }
        
        public IEnumerable<IResponseModifier> Modifiers { get; set; }
        
        public IExecutorFactory ExecutorFactory { get; set; }
        
        public IPageLinkCalculator PageLinkCalculator { get; set; }
        
        public ICommandStrategySets Strategies { get; set; }
        
        public INamingConventionSwitcher NameSwitcher { get; set; }
        
        public IEndpointResolver EndpointResolver { get; set; }
        
        public IUrlHelper UrlHelper { get; set; }
    }
}