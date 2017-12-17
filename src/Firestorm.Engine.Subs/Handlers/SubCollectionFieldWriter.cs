using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubCollectionFieldWriter<TItem, TProperty, TNav> : IFieldWriter<TItem>
        where TItem : class
        where TNav : class, new()
        where TProperty : IEnumerable<TNav>
    {
        private readonly Expression<Func<TItem, TProperty>> _navigationExpression;
        private readonly IEngineSubContext<TNav> _substemSubContext;

        public SubCollectionFieldWriter(Expression<Func<TItem, TProperty>> navigationExpression, IEngineSubContext<TNav> substemSubContext)
        {
            _navigationExpression = navigationExpression;
            _substemSubContext = substemSubContext;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            var navRepository = new NavigationCollectionRepository<TItem, TProperty, TNav>(item, _navigationExpression);

            var navLocatorCreator = new NavigationItemLocatorCreator<TNav>(_substemSubContext);
            var navContext = new FullEngineContext<TNav>(dataTransaction, navRepository, _substemSubContext);

            IEnumerable deserializedCollection = (IEnumerable) deserializedValue; // todo null ?

            foreach (object deserializedItem in deserializedCollection)
            {
                var itemData = new RestItemData(deserializedItem);
                
                DeferredItemBase<TNav> deferredItem = await navLocatorCreator.LocateOrCreateItemAsync(navContext, itemData, item.LoadAsync);

                var navEngineItem = new EngineRestItem<TNav>(navContext, deferredItem);
                Acknowledgment acknowledgment = await navEngineItem.EditAsync(itemData);
            }

            // TODO: remove items that were not located?
        }
    }
}