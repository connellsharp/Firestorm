using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;
using Firestorm.Engine.Subs.Wrapping;
using Firestorm.Fluent.Fuel.Engine;
using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Sources
{
    internal class RestCollectionSource<TItem> : IRestCollectionSource
        where TItem : class, new()
    {
        private readonly IDataSource _dataSource;
        private readonly ActionDataChangeEvents<TItem> _events;
        private readonly FluentEngineSubContext<TItem> _subContext;

        public RestCollectionSource(IDataSource dataSource, ApiItemModel<TItem> itemModel)
        {
            _dataSource = dataSource;
            _events = itemModel.Events;
            _subContext = new FluentEngineSubContext<TItem>(itemModel);
        }

        public IRestCollection GetRestCollection()
        {
            IDataTransaction transaction = _dataSource.CreateTransaction();
            // TODO setup disposing of transaction

            IEngineRepository<TItem> repository = _dataSource.GetRepository<TItem>(transaction);

            var wrapper = new DataEventWrapper<TItem>(transaction, repository);
            wrapper.TryWrapEvents(_events);

            IEngineContext<TItem> context = new FullEngineContext<TItem>(wrapper.Transaction, wrapper.Repository, _subContext);

            return new EngineRestCollection<TItem>(context);
        }
    }
}