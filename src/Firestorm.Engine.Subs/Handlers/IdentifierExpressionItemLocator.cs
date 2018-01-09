using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Data;
using Firestorm.Engine.Subs.Context;

namespace Firestorm.Engine.Subs.Handlers
{
    public class IdentifierExpressionItemLocator<TItem, TValue> : IItemLocator<TItem>
        where TItem : class
    {
        private readonly Expression<Func<TItem, TValue>> _getLocatorValueExpression;

        public IdentifierExpressionItemLocator(Expression<Func<TItem, TValue>> getLocatorValueExpression)
        {
            _getLocatorValueExpression = getLocatorValueExpression;
        }

        public TItem TryLocateItem(IEngineRepository<TItem> navRepository, object findValue)
        {
            TValue convertedValue = ConversionUtility.ConvertValue<TValue>(findValue);
            var predicate = IdentifierExpressionHelpers.GetIdentifierPredicate(_getLocatorValueExpression, convertedValue);
            return navRepository.GetAllItems().Where(predicate).SingleOrDefault();
        }
    }
}