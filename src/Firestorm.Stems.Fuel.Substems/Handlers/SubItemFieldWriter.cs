using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Fuel.Substems.Repositories;

namespace Firestorm.Stems.Fuel.Substems.Handlers
{
    internal class SubItemFieldWriter<TItem, TNav> : IFieldWriter<TItem>
        where TItem : class
        where TNav : class, new()
    {
        private readonly Expression<Func<TItem, TNav>> _navigationExpression;
        private readonly StemEngineSubContext<TNav> _engineSubContext;

        internal SubItemFieldWriter(Expression<Func<TItem, TNav>> navigationExpression, StemEngineSubContext<TNav> engineSubContext)
        {
            _navigationExpression = navigationExpression;
            _engineSubContext = engineSubContext;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            IQueryableSingle<TNav> navigationQuery = item.Query.Select(_navigationExpression).SingleDefferred();
            var navSingleRepository = new QueryableSingleRepository<TNav>(navigationQuery);

            var navContext = new StemEngineContext<TNav>(dataTransaction, navSingleRepository, _engineSubContext);

            var itemData = new RestItemData(deserializedValue);

            //var navLocatorCreator = new StemNavigationItemLocatorCreator<TNav>(_engineSubContext);
            //DeferredItemBase<TNav> deferredItem = navLocatorCreator.LocateItem(navContext, itemData);

            DeferredItemBase<TNav> deferredItem = new RepositoryDeferredItem<TNav>(navSingleRepository);

           var navEngineItem = new EngineRestItem<TNav>(navContext, deferredItem);
            Acknowledgment acknowledgment = await navEngineItem.EditAsync(itemData);
        }
    }
}