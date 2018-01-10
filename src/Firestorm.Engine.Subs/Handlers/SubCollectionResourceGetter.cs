using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Data;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;
using JetBrains.Annotations;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubCollectionResourceGetter<TItem, TCollection, TNav> : IFieldResourceGetter<TItem>
        where TItem : class
        where TNav : class, new()
        where TCollection : IEnumerable<TNav>
    {
        private readonly Expression<Func<TItem, TCollection>> _navigationExpression;
        private readonly IEngineSubContext<TNav> _engineSubContext;
        private readonly IRepositoryEvents<TNav> _repoEvents;

        public SubCollectionResourceGetter(Expression<Func<TItem, TCollection>> navigationExpression, IEngineSubContext<TNav> engineSubContext, [CanBeNull] IRepositoryEvents<TNav> repoEvents)
        {
            _navigationExpression = navigationExpression;
            _engineSubContext = engineSubContext;
            _repoEvents = repoEvents;
        }

        public IRestResource GetFullResource(IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            IEngineRepository<TNav> navRepository = new NavigationCollectionRepository<TItem, TCollection, TNav>(item, _navigationExpression);
            navRepository = RepositoryWrapperUtility.TryWrapEvents(navRepository, _repoEvents);

            var context = new FullEngineContext<TNav>(dataTransaction, navRepository, _engineSubContext);

            return new EngineRestCollection<TNav>(context);
        }
    }
}