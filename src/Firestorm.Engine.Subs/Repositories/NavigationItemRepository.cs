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
        where TParent : class
        where TNav : class, new()
    {
        private readonly IDeferredItem<TParent> _parentItem;
        private readonly Expression<Func<TParent, TNav>> _navExpression;
        private readonly INavigationSetter<TParent, TNav> _setter;

        public NavigationItemRepository(IDeferredItem<TParent> parentItem, Expression<Func<TParent, TNav>> navExpression, INavigationSetter<TParent, TNav> setter)
        {
            _parentItem = parentItem;
            _navExpression = navExpression;
            _setter = setter;
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
            return _parentItem.Query.Select(_navExpression).Where(i => i != null);
        }

        public TNav CreateAndAttachItem()
        {
            var item = new TNav();
            SetParentNavItem(item);
            return item;
        }

        public void MarkUpdated(TNav item)
        {
        }

        public void MarkDeleted(TNav item)
        {
            SetParentNavItem(null);
        }

        private void SetParentNavItem(TNav item)
        {
            var parent = _parentItem.Query.First();
            _setter.SetNavItem(parent, item);
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}