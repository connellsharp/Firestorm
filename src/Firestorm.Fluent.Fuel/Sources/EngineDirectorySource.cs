using Firestorm.Fluent.Fuel.Definitions;

namespace Firestorm.Fluent.Fuel.Sources
{
    public class EngineDirectorySource : IApiDirectorySource
    {
        private readonly EngineApiModel _model;

        public EngineDirectorySource(EngineApiModel model)
        {
            _model = model;
        }

        public IRestCollectionSource GetCollectionSource(string collectionName)
        {
            return _model.Items[collectionName].GetCollectionSource();
        }
    }
}