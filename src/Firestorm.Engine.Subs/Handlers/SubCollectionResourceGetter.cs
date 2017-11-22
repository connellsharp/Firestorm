using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Stems.Fuel.Substems.Repositories;

namespace Firestorm.Stems.Fuel.Substems.Handlers
{
    public class SubCollectionResourceGetter<TItem, TCollection, TNav> : IFieldResourceGetter<TItem>
        where TItem : class
        where TNav : class, new()
        where TCollection : IEnumerable<TNav>
    {
        private readonly Expression<Func<TItem, TCollection>> _navigationExpression;
        private readonly IEngineSubContext<TNav> _engineSubContext;

        public SubCollectionResourceGetter(Expression<Func<TItem, TCollection>> navigationExpression, IEngineSubContext<TNav> engineSubContext)
        {
            _navigationExpression = navigationExpression;
            _engineSubContext = engineSubContext;
        }

        public IRestResource GetFullResource(IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            var navRepository = new NavigationCollectionRepository<TItem, TCollection, TNav>(item, _navigationExpression);
            var context = new AdditiveEngineContext<TNav>(dataTransaction, navRepository, _engineSubContext);
            return new EngineRestCollection<TNav>(context);
        }
    }
}