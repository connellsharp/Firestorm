using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public NavigationItemRepository(IDeferredItem<TParent> parentItem, Expression<Func<TParent, TNav>> navigationExpression)
        {
            _parentItem = parentItem;
            _navigationExpression = navigationExpression;
        }

        public async Task InitializeAsync()
        {
            // currently this is the only time this method is used.. perhaps this can be combined with CreateAndAttachItem ?
            await _parentItem.LoadAsync();
        }

        public IQueryable<TNav> GetAllItems()
        {
            return _parentItem.Query.Select(_navigationExpression);
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
            throw new NotImplementedException("Not implemented creating new items in the NavigationItemRepository yet.");

            // TODO not just properties here. Use the writers somehow?
            //PropertyInfo property = PropertyInfoUtilities.GetPropertyInfoFromLambda(_navigationExpression);
            //property.SetValue(parent, item);
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}