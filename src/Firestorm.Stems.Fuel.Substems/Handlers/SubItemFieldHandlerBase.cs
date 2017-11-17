using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Fuel.Substems.Repositories;

namespace Firestorm.Stems.Fuel.Substems.Handlers
{
    internal abstract class SubItemFieldHandlerBase<TItem, TNav> : IFieldHandler<TItem>
        where TNav : class
    {
        protected readonly Expression<Func<TItem, TNav>> NavigationExpression;
        protected readonly StemEngineSubContext<TNav> EngineSubContext;

        protected SubItemFieldHandlerBase(Expression<Func<TItem, TNav>> navigationExpression, StemEngineSubContext<TNav> engineSubContext)
        {
            NavigationExpression = navigationExpression;
            EngineSubContext = engineSubContext;
        }

        protected EngineRestItem<TNav> GetQueriedEngineItem(IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            IQueryableSingle<TNav> navigationQuery = item.Query.Select(NavigationExpression).SingleDefferred();
            var itemRepository = new QueryableSingleRepository<TNav>(navigationQuery);
            var context = new StemEngineContext<TNav>(dataTransaction, itemRepository, EngineSubContext);
            var deferredNavItem = new RepositoryDeferredItem<TNav>(itemRepository);
            return new EngineRestItem<TNav>(context, deferredNavItem);
        }

        // I wonder if this is necessary. Could the QueriedEngineItem work for all cases?
        protected EngineRestItem<TNav> GetLoadedEngineItem(IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            TNav navItem = NavigationExpression.Compile().Invoke(item.LoadedItem);
            var repository = new LoadedItemRepository<TNav>(navItem);
            var context = new StemEngineContext<TNav>(dataTransaction, repository, EngineSubContext);
            var deferredNavItem = new RepositoryDeferredItem<TNav>(context.Repository);
            return new EngineRestItem<TNav>(context, deferredNavItem);
        }
    }
}