using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;

namespace Firestorm.Fluent.Start
{
    public class ApiContextStartResourceFactory : IStartResourceFactory
    {
        private readonly ConcurrentDictionary<string, IFluentCollectionCreator> _creators = new ConcurrentDictionary<string, IFluentCollectionCreator>();

        public ApiContext ApiContext { get; set; }

        public void Initialize()
        {
            var sourceCreator = new SourceCreator();
            var source = sourceCreator.CreateSource(ApiContext);

            foreach (IApiCollectionSource itemSet in source.GetRootSources())
            {
                _creators.TryAdd(itemSet.Name, itemSet.GetCollectionCreator());
            }
        }

        public IRestResource GetStartResource(IRestEndpointContext endpointContext)
        {
            return new ApiContextDirectory(endpointContext, _creators);
        }
    }

    public class ApiContextDirectory : IRestDirectory
    {
        private readonly IDictionary<string, IFluentCollectionCreator> _creatorCache;

        internal ApiContextDirectory(IRestEndpointContext apiContext, IDictionary<string, IFluentCollectionCreator> creatorCache)
        {
            _creatorCache = creatorCache;
        }

        public IRestResource GetChild(string startResourceName)
        {
            if (!_creatorCache.ContainsKey(startResourceName))
                return null;

            IFluentCollectionCreator creator = _creatorCache[startResourceName];

            return creator.GetRestCollection();
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
