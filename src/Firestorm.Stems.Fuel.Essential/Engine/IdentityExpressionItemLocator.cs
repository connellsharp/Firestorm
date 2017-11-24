using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Stems.Fuel.Fields;

namespace Firestorm.Stems.Fuel.Essential
{
    internal class IdentityExpressionItemLocator<TItem, TValue> : IItemLocator<TItem>
        where TItem : class
    {
        private readonly Expression<Func<TItem, TValue>> _getLocatorValueExpression;

        public IdentityExpressionItemLocator(Expression<Func<TItem, TValue>> getLocatorValueExpression)
        {
            _getLocatorValueExpression = getLocatorValueExpression;
        }

        public TItem TryLocateItem(IEngineRepository<TItem> navRepository, object findValue)
        {
            var predicate = IdentifierExpressionHelpers.GetIdentifierPredicate(_getLocatorValueExpression, findValue.ToString());
            return navRepository.GetAllItems().Where(predicate).SingleOrDefault();
        }
    }
}