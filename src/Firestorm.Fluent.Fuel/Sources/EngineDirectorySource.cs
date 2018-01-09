using System.Collections.Generic;
using System.Linq;
using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Sources
{
    public class EngineDirectorySource : IApiDirectorySource
    {
        private readonly Dictionary<string, IApiRoot> _rootItems;

        public EngineDirectorySource(EngineApiModel model)
        {
            _rootItems = model.Roots.ToDictionary(r => r.RootName);
        }

        public IRestCollectionSource GetCollectionSource(string collectionName)
        {
            IApiRoot itemModel = _rootItems[collectionName];
            return itemModel.GetRootCollectionSource();
        }

        public IEnumerable<string> GetCollectionNames()
        {
            return _rootItems.Keys;
        }
    }
}