using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// An engine repository for a navigation collection property.
    /// </summary>
    internal class NavigationItemRepository<TParent, TNav> : IEngineRepository<TNav>
        where TNav : class, new()
    {
        private readonly IDeferredItem<TParent> _parentItem;
        private readonly Expression<Func<TParent, TNav>> _navigationExpression;
        private readonly INavigationItemSetter<TParent, TNav> _navSetter;

        public NavigationItemRepository(IDeferredItem<TParent> parentItem, Expression<Func<TParent, TNav>> navigationExpression, INavigationItemSetter<TParent, TNav> navSetter)
        {
            _parentItem = parentItem;
            _navigationExpression = navigationExpression;
            _navSetter = navSetter;
        }

        public async Task InitializeAsync()
        {
            // currently this is the only time this method is used.. perhaps this can be combined with CreateAndAttachItem ?
            await _parentItem.LoadAsync();
        }

        public IQueryable<TNav> GetAllItems()
        {
            // assumes that parent item query always 1 item
            // returns empty enumerable if no navigation item is set so CreateAndAttachItem can be called later.
            return _parentItem.Query.Select(_navigationExpression).Where(i => i != null);
        }

        public TNav CreateAndAttachItem()
        {
            var item = new TNav();
            SetParentNavItem(item);
            return item;
        }

        public void MarkDeleted(TNav item)
        {
            SetParentNavItem(null);
        }

        private void SetParentNavItem(TNav item)
        {
            var parent = _parentItem.Query.First();
            _navSetter.SetNavItem(parent, item);
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}