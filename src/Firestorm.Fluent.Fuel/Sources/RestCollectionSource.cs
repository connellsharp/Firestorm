using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Subs.Context;
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
            IDataContext<TItem> dataContext = _dataSource.CreateContext<TItem>();
            // TODO setup disposing of transaction

            IEngineContext<TItem> context = _subContext.CreateFullContext(dataContext);

            return new EngineRestCollection<TItem>(context);
        }
    }
}