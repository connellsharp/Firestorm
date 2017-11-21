using System.Collections.Concurrent;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Firestorm.Engine.Data;
using Firestorm.Fluent.Fuel;

namespace Firestorm.Fluent.Start
{
    public class FluentStartResourceFactory : IStartResourceFactory
    {
        private readonly ConcurrentDictionary<string, IFluentCollectionCreator> _creators = new ConcurrentDictionary<string, IFluentCollectionCreator>();

        public ApiContext ApiContext { get; set; }

        public IDataSource DataSource { get; set; }

        public void Initialize()
        {
            var sourceCreator = new SourceCreator();
            var creatorCreator = new FluentCollectionCreatorCreator(DataSource);
            var apiSource = sourceCreator.CreateSource(ApiContext, creatorCreator);

            foreach (var rootSource in apiSource.GetRootSources())
            {
                _creators.TryAdd(rootSource.Name, rootSource.CollectionCreator);
            }
        }

        public IRestResource GetStartResource(IRestEndpointContext endpointContext)
        {
            return new ApiContextDirectory(endpointContext, _creators);
        }
    }
}