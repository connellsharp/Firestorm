using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Data;
using Firestorm.Engine.Subs.Context;

namespace Firestorm.Engine.Subs.Handlers
{
    /// <summary>
    /// Locates an item in a repository by finding the single item that matches the given value.
    /// Should be used with properties where a unique key ensures no more than one item can have the same value.
    /// </summary>
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