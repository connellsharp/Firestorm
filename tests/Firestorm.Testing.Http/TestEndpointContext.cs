using System.Collections.Generic;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Responses;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Testing.Http
{
    public class TestEndpointContext : IEndpointContext
    {
        private readonly EndpointServices _services;
        private readonly TestRequestContext _request;

        public TestEndpointContext()
        {
            var builder = new DefaultServicesBuilder();
            builder.AddEndpoints();
            
            _services = builder.Build().GetService<EndpointServices>();
            
            _request = new TestRequestContext();
        }

        public IEndpointCoreServices Services => _services;

        public IRequestContext Request => _request;
        
        public IEnumerable<IResponseModifier> Modifiers => _services.Modifiers;
    }
}