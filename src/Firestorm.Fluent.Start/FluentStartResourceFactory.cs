using System.Collections.Concurrent;
using Firestorm.Data;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Firestorm.Fluent.Fuel;
using Firestorm.Fluent.Fuel.Builder;
using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Start
{
    public class FluentStartResourceFactory : IStartResourceFactory
    {
        private IApiDirectorySource _apiDirectorySource;
        public ApiContext ApiContext { get; set; }

        public IDataSource DataSource { get; set; }

        public void Initialize()
        {
            EngineApiModel engineModel = new EngineApiModel();
            var builder = new EngineApiBuilder(engineModel);

            var sourceCreator = new SourceCreator();
            _apiDirectorySource = sourceCreator.CreateSource(ApiContext, builder);
        }

        public IRestResource GetStartResource(IRestEndpointContext endpointContext)
        {
            return new ApiContextDirectory(endpointContext, _apiDirectorySource);
        }
    }
}