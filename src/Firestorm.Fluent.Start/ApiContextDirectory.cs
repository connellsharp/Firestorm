using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Endpoints;

namespace Firestorm.Fluent.Start
{
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