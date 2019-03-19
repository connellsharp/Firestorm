using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubItemResourceGetter<TItem, TNav> : IFieldResourceGetter<TItem>
        where TItem : class
        where TNav : class, new()
    {
        private readonly Expression<Func<TItem, TNav>> _expression;
        private readonly IEngineSubContext<TNav> _engineSubContext;

        public SubItemResourceGetter(SubWriterTools<TItem, TNav, TNav> navTools, IEngineSubContext<TNav> engineSubContext)
        {
            _expression = navTools.NavExpression; // TODO just expression
            _engineSubContext = engineSubContext;
        }

        public IRestResource GetFullResource(IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            //TNav navItem = _expression.Compile().Invoke(item.LoadedItem);
            //var itemRepository = new LoadedItemRepository<TNav>(navItem);
            IQueryableSingle<TNav> navigationQuery = item.Query.Select(_expression).SingleDefferred();
            var itemRepository = new QueryableSingleRepository<TNav>(navigationQuery);
            var deferredNavItem = new RepositoryDeferredItem<TNav>(itemRepository, new MemoryAsyncQueryer());

            var context = _engineSubContext.CreateFullContext(new DataContext<TNav>
            {
                Repository = itemRepository,
                AsyncQueryer = new MemoryAsyncQueryer(),
                Transaction = dataTransaction
            });
            
            return new EngineRestItem<TNav>(context, deferredNavItem);
        }
    }
}