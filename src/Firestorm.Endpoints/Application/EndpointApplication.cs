using System.Collections.Generic;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Requests;
using Firestorm.Endpoints.Responses;
using Firestorm.Host;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// All the services required to run a Firestorm Endpoints application.
    /// </summary>
    public class EndpointApplication : IEndpointServices
    {
        public IStartResourceFactory StartResourceFactory { get; set; }
        
        public IQueryCreator QueryCreator { get; set; }
        
        public IEnumerable<IResponseModifier> Modifiers { get; set; }
        
        public IExecutorFactory ExecutorFactory { get; set; }
        
        public IPageLinkCalculator PageLinkCalculator { get; set; }
        
        public ICommandStrategySets Strategies { get; set; }
        
        public INamingConventionSwitcher NameSwitcher { get; set; }
        
        public IEndpointResolver EndpointResolver { get; set; }
    }
}