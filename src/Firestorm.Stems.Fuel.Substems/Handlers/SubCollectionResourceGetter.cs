using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Stems.Fuel;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Stems.Fuel.Substems.Repositories;

namespace Firestorm.Stems.Power.Substems
{
    internal class SubCollectionResourceGetter<TItem, TNav> : IFieldResourceGetter<TItem>
        where TItem : class
        where TNav : class, new()
    {
        private readonly Expression<Func<TItem, IEnumerable<TNav>>> _navigationExpression;
        private readonly StemEngineSubContext<TNav> _engineSubContext;

        public SubCollectionResourceGetter(Expression<Func<TItem, IEnumerable<TNav>>> navigationExpression, StemEngineSubContext<TNav> engineSubContext)
        {
            _navigationExpression = navigationExpression;
            _engineSubContext = engineSubContext;
        }

        public IRestResource GetFullResource(IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            var navRepository = new NavigationCollectionRepository<TItem, TNav>(item, _navigationExpression);
            var context = new StemEngineContext<TNav>(dataTransaction, navRepository, _engineSubContext);
            return new EngineRestCollection<TNav>(context);
        }
    }
}