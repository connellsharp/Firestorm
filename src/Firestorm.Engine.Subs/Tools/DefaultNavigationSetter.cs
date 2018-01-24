using System;
using System.Linq.Expressions;

namespace Firestorm.Engine.Subs
{
    public class DefaultNavigationSetter<TParent, TNav> : INavigationSetter<TParent,TNav>
    {
        private readonly Expression<Func<TParent, TNav>> _navigationExpression;

        public DefaultNavigationSetter(Expression<Func<TParent, TNav>> navigationExpression)
        {
            _navigationExpression = navigationExpression;
        }

        public void SetNavItem(TParent parent, TNav item)
        {
            // TODO rename? not really anything to do with identifiers
            IdentifierExpressionHelpers.SetIdentifier(parent, _navigationExpression, item);
        }
    }
}