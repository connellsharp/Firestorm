using System;
using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Subs.Context;
using Firestorm.Fluent.Fuel.Engine;
using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Sources
{
    internal class RestCollectionSource<TItem> : IRestCollectionSource
        where TItem : class, new()
    {
        private readonly IDataSource _dataSource;
        private readonly Action<TItem> _onCreating;
        private readonly FluentEngineSubContext<TItem> _subContext;

        public RestCollectionSource(IDataSource dataSource, ApiItemModel<TItem> itemModel)
        {
            _dataSource = dataSource;
            _onCreating = itemModel.OnCreating;
            _subContext = new FluentEngineSubContext<TItem>(itemModel);
        }

        public IRestCollection GetRestCollection()
        {
            IDataTransaction transaction = _dataSource.CreateTransaction();
            // TODO setup disposing of transaction

            IEngineRepository<TItem> repository = _dataSource.GetRepository<TItem>(transaction);
            var wrapperRepository = new RepositoryWrapper<TItem>(repository, _onCreating);

            IEngineContext<TItem> context = new FullEngineContext<TItem>(transaction, wrapperRepository, _subContext);
            return new EngineRestCollection<TItem>(context);
        }
    }
}