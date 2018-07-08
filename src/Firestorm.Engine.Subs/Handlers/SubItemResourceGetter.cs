using System.Linq;
using Firestorm.Data;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;
using Firestorm.Engine.Subs.Wrapping;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubItemResourceGetter<TItem, TNav> : IFieldResourceGetter<TItem>
        where TItem : class
        where TNav : class, new()
    {
        private readonly SubWriterTools<TItem, TNav, TNav> _navTools;
        private readonly IEngineSubContext<TNav> _engineSubContext;

        public SubItemResourceGetter(SubWriterTools<TItem, TNav, TNav> navTools, IEngineSubContext<TNav> engineSubContext)
        {
            _navTools = navTools;
            _engineSubContext = engineSubContext;
        }

        public IRestResource GetFullResource(IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            //TNav navItem = _navTools.NavExpression.Compile().Invoke(item.LoadedItem);
            //var itemRepository = new LoadedItemRepository<TNav>(navItem);
            IQueryableSingle<TNav> navigationQuery = item.Query.Select(_navTools.NavExpression).SingleDefferred();
            var itemRepository = new QueryableSingleRepository<TNav>(navigationQuery);

            var eventWrapper = new DataEventWrapper<TNav>(dataTransaction, itemRepository);
            eventWrapper.TryWrapEvents(_navTools.RepoEvents);

            var context = new FullEngineContext<TNav>(eventWrapper.Transaction, eventWrapper.Repository, _engineSubContext);
            var deferredNavItem = new RepositoryDeferredItem<TNav>(itemRepository);
            return new EngineRestItem<TNav>(context, deferredNavItem);
        }
    }
}