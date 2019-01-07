using Firestorm.Data;
using Firestorm.Fluent.Fuel.Builder;
using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Sources;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Fluent
{
    internal class FluentStartResourceFactory : IStartResourceFactory
    {
        private IApiDirectorySource _apiDirectorySource;

        public IApiContext ApiContext { get; set; }

        public IDataSource DataSource { get; set; }

        public void Initialize()
        {
            var engineModel = new EngineApiModel(DataSource);
            var builder = new EngineApiBuilder(engineModel);

            ApiContext.CreateApi(builder);
            
            _apiDirectorySource = builder.BuildSource();
        }

        public IRestResource GetStartResource(IRequestContext requestContext)
        {
            if(_apiDirectorySource == null)
                Initialize();

            return new ApiContextDirectory(requestContext, _apiDirectorySource);
        }
    }
}