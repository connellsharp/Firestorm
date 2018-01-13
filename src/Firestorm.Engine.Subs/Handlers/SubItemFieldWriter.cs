using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubItemFieldWriter<TItem, TNav> : IFieldWriter<TItem>
        where TItem : class
        where TNav : class, new()
    {
        private readonly Expression<Func<TItem, TNav>> _navigationExpression;
        private readonly IEngineSubContext<TNav> _engineSubContext;
        private readonly IRepositoryEvents<TNav> _repoEvents;

        public SubItemFieldWriter(Expression<Func<TItem, TNav>> navigationExpression, IEngineSubContext<TNav> engineSubContext, IRepositoryEvents<TNav> repoEvents)
        {
            _navigationExpression = navigationExpression;
            _engineSubContext = engineSubContext;
            _repoEvents = repoEvents;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            //IQueryableSingle<TNav> navigationQuery = item.Query.Select(_navigationExpression).SingleDefferred();
            //IEngineRepository<TNav> navRepository = new QueryableSingleRepository<TNav>(navigationQuery);
            IEngineRepository<TNav> navRepository = new NavigationItemRepository<TItem, TNav>(item, _navigationExpression);
            navRepository = RepositoryWrapperUtility.TryWrapEvents(navRepository, _repoEvents);
            
            var itemData = new RestItemData(deserializedValue);

            var navLocatorCreator = new NavigationItemLocatorCreator<TNav>(_engineSubContext);
            DeferredItemBase<TNav> deferredItem = await navLocatorCreator.LocateOrCreateItemAsync(navRepository, itemData, item.LoadAsync);
            //DeferredItemBase<TNav> deferredItem = new RepositoryDeferredItem<TNav>(navSingleRepository);

            var navContext = new FullEngineContext<TNav>(dataTransaction, navRepository, _engineSubContext);
            var navEngineItem = new EngineRestItem<TNav>(navContext, deferredItem);
            Acknowledgment acknowledgment = await navEngineItem.EditAsync(itemData);
        }
    }
}