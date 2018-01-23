using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Engine.Subs.Handlers
{
    public class DefaultNavigationItemSetter<TParent, TNav> : INavigationItemSetter<TParent,TNav>
    {
        private readonly Expression<Func<TParent, TNav>> _navigationExpression;

        public DefaultNavigationItemSetter(Expression<Func<TParent, TNav>> navigationExpression)
        {
            _navigationExpression = navigationExpression;
        }

        public void SetNavItem(TParent parent, TNav item)
        {
            throw new NotImplementedException("Not implemented creating new items in the NavigationItemRepository yet.");

            // TODO not just properties here. Use the writers somehow?
            //PropertyInfo property = PropertyInfoUtilities.GetPropertyInfoFromLambda(_navigationExpression);
            //property.SetValue(parent, item);
        }
    }
}