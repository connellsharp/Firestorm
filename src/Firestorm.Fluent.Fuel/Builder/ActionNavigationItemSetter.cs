using System;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Fluent.Fuel.Builder
{
    internal class ActionNavigationItemSetter<TParent, TNav> : INavigationItemSetter<TParent, TNav>
    {
        private readonly Action<TParent, TNav> _action;

        public ActionNavigationItemSetter(Action<TParent, TNav> action)
        {
            _action = action;
        }

        public void SetNavItem(TParent parent, TNav item)
        {
            _action(parent, item);
        }
    }
}