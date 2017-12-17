using System.Collections.Generic;
using System.Linq;
using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Sources
{
    public class EngineDirectorySource : IApiDirectorySource
    {
        private readonly EngineApiModel _model;
        private readonly Dictionary<string, IApiItemModel> _rootItems;

        public EngineDirectorySource(EngineApiModel model)
        {
            _model = model;
            _rootItems = _model.Items.Where(im => im.RootName != null).ToDictionary(im => im.RootName);
        }

        public IRestCollectionSource GetCollectionSource(string collectionName)
        {
            return _rootItems[collectionName].GetCollectionSource();
        }
    }
}