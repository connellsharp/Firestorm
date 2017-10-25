using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine;

namespace Firestorm.Stems.Fuel.Substems.Repositories
{
    /// <summary>
    /// An engine repository for a navigation collection property.
    /// </summary>
    public class NavigationCollectionRepository<TParent, TNav> : IEngineRepository<TNav>
        where TNav : class, new()
    {
        private readonly IDeferredItem<TParent> _parentItem;
        private readonly Expression<Func<TParent, IEnumerable<TNav>>> _navigationExpression;

        public NavigationCollectionRepository(IDeferredItem<TParent> parentItem, Expression<Func<TParent, IEnumerable<TNav>>> navigationExpression)
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
            return _parentItem.Query.SelectMany(_navigationExpression);
        }

        public TNav CreateAndAttachItem()
        {
            ICollection<TNav> navCollection = GetNavCollection();

            var item = new TNav();
            navCollection.Add(item);
            return item;
        }

        public void MarkDeleted(TNav item)
        {
            ICollection<TNav> navCollection = GetNavCollection();
            navCollection.Remove(item);
        }

        private ICollection<TNav> GetNavCollection()
        {
            TParent parentItem = _parentItem.LoadedItem;
            IEnumerable<TNav> navEnumerable = _navigationExpression.Compile().Invoke(parentItem);

            var navCollection = navEnumerable as ICollection<TNav>;
            if (navCollection == null)
                throw new NotSupportedException("Cannot add items to this navigation property.");

            return navCollection;
        }

        /// <summary>
        /// This is used because entity framework does not support async enumerating on navigation collections.
        /// </summary>
        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}