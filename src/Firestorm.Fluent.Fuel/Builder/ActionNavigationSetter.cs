using System;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Fluent.Fuel.Builder
{
    internal class ActionNavigationSetter<TParent, TNav> : INavigationSetter<TParent, TNav>
    {
        private readonly Action<TParent, TNav> _action;

        public ActionNavigationSetter(Action<TParent, TNav> action)
        {
            _action = action;
        }

        public void SetNavItem(TParent parent, TNav item)
        {
            _action(parent, item);
        }
    }
}