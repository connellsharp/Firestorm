using System.Collections.Concurrent;
using Firestorm.Data;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Firestorm.Fluent.Fuel.Builder;
using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Start
{
    public class FluentStartResourceFactory : IStartResourceFactory
    {
        private IApiDirectorySource _apiDirectorySource;

        public RestContext RestContext { get; set; }

        public IDataSource DataSource { get; set; }

        public void Initialize()
        {
            var engineModel = new EngineApiModel(DataSource);
            var builder = new EngineApiBuilder(engineModel);

            var sourceCreator = new SourceCreator();
            _apiDirectorySource = sourceCreator.CreateSource(RestContext, builder);
        }

        public IRestResource GetStartResource(IRestEndpointContext endpointContext)
        {
            if(_apiDirectorySource == null)
                Initialize();

            return new ApiContextDirectory(endpointContext, _apiDirectorySource);
        }
    }
}