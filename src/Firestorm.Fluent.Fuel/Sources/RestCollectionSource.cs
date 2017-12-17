using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Fluent.Fuel.Engine;
using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Sources
{
    internal class RestCollectionSource<TItem> : IRestCollectionSource
        where TItem : class, new()
    {
        private readonly IDataSource _dataSource;
        private readonly IDictionary<string, ApiFieldModel<TItem>> _fieldModels;
        private readonly IDictionary<string, ApiIdentifierModel<TItem>> _identifierModels;

        internal RestCollectionSource(IDataSource dataSource, IDictionary<string, ApiFieldModel<TItem>> fieldModels, IDictionary<string, ApiIdentifierModel<TItem>> identifierModels)
        {
            _dataSource = dataSource;
            _fieldModels = fieldModels;
            _identifierModels = identifierModels;
        }

        public IRestCollection GetRestCollection()
        {
            IEngineContext<TItem> context = new FluentEngineContext<TItem>(_dataSource, _fieldModels, _identifierModels);
            return new EngineRestCollection<TItem>(context);
        }
    }
}