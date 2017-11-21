using System.Collections.Generic;
using Firestorm.Engine;
using Firestorm.Engine.Data;
using Firestorm.Fluent.Fuel.Definitions;

namespace Firestorm.Fluent.Fuel.Sources
{
    internal class RestCollectionSource<TItem> : IRestCollectionSource
        where TItem : class
    {
        private readonly IDictionary<string, ApiFieldModel<TItem>> _fieldModels;

        internal RestCollectionSource(IDictionary<string, ApiFieldModel<TItem>> fieldModels)
        {
            _fieldModels = fieldModels;
        }

        public IRestCollection GetRestCollection()
        {
            IEngineContext<TItem> context = new FluentEngineContext<TItem>(_fieldModels);
            return new EngineRestCollection<TItem>(context);
        }
    }
}