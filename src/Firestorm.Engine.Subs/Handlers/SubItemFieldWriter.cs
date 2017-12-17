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
        where TNav : class
    {
        private readonly Expression<Func<TItem, TNav>> _navigationExpression;
        private readonly IEngineSubContext<TNav> _engineSubContext;

        public SubItemFieldWriter(Expression<Func<TItem, TNav>> navigationExpression, IEngineSubContext<TNav> engineSubContext)
        {
            _navigationExpression = navigationExpression;
            _engineSubContext = engineSubContext;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            IQueryableSingle<TNav> navigationQuery = item.Query.Select(_navigationExpression).SingleDefferred();
            var navSingleRepository = new QueryableSingleRepository<TNav>(navigationQuery);

            var navContext = new FullEngineContext<TNav>(dataTransaction, navSingleRepository, _engineSubContext);

            var itemData = new RestItemData(deserializedValue);

            //var navLocatorCreator = new NavigationItemLocatorCreator<TNav>(_engineSubContext);
            //DeferredItemBase<TNav> deferredItem = navLocatorCreator.LocateItem(navContext, itemData);

            DeferredItemBase<TNav> deferredItem = new RepositoryDeferredItem<TNav>(navSingleRepository);

           var navEngineItem = new EngineRestItem<TNav>(navContext, deferredItem);
            Acknowledgment acknowledgment = await navEngineItem.EditAsync(itemData);
        }
    }
}