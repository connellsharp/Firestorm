using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;
using JetBrains.Annotations;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubCollectionFieldWriter<TItem, TProperty, TNav> : IFieldWriter<TItem>
        where TItem : class
        where TNav : class, new()
        where TProperty : IEnumerable<TNav>
    {
        private readonly Expression<Func<TItem, TProperty>> _navigationExpression;
        private readonly IEngineSubContext<TNav> _subContext;
        private readonly IRepositoryEvents<TNav> _repoEvents;
        private readonly INavigationSetter<TItem, TProperty> _setter;

        public SubCollectionFieldWriter(Expression<Func<TItem, TProperty>> navigationExpression, IEngineSubContext<TNav> subContext, IRepositoryEvents<TNav> repoEvents, [CanBeNull] INavigationSetter<TItem, TProperty> setter)
        {
            _navigationExpression = navigationExpression;
            _subContext = subContext;
            _repoEvents = repoEvents;
            _setter = setter ?? new DefaultNavigationSetter<TItem, TProperty>(_navigationExpression);
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            IEngineRepository<TNav> navRepository = new NavigationCollectionRepository<TItem, TProperty, TNav>(item, _navigationExpression, _setter);
            navRepository = RepositoryWrapperUtility.TryWrapEvents(navRepository, _repoEvents);

            IDataTransaction transaction = new VoidTransaction(); // we commit the transaction in the parent. TODO optional save-as-you-go ?
            var navContext = new FullEngineContext<TNav>(transaction, navRepository, _subContext);

            var navLocatorCreator = new NavigationItemLocatorCreator<TNav>(_subContext);

            IEnumerable deserializedCollection = (IEnumerable) deserializedValue; // todo null ?

            foreach (object deserializedItem in deserializedCollection)
            {
                var itemData = new RestItemData(deserializedItem);
                
                DeferredItemBase<TNav> deferredItem = await navLocatorCreator.LocateOrCreateItemAsync(navRepository, itemData, item.LoadAsync);

                var navEngineItem = new EngineRestItem<TNav>(navContext, deferredItem);
                Acknowledgment acknowledgment = await navEngineItem.EditAsync(itemData);
            }

            // TODO: remove items that were not located?
        }
    }
}