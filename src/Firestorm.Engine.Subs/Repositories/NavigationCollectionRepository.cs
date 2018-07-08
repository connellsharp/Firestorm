using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// An engine repository for a navigation collection property.
    /// </summary>
    internal class NavigationCollectionRepository<TParent, TCollection, TNav> : IEngineRepository<TNav>
        where TParent : class
        where TNav : class, new()
        where TCollection : class, IEnumerable<TNav>
    {
        private readonly IDeferredItem<TParent> _parentItem;
        private readonly SubWriterTools<TParent, TCollection, TNav> _tools;
        private readonly Expression<Func<TParent, IEnumerable<TNav>>> _castedExpression;

        public NavigationCollectionRepository(IDeferredItem<TParent> parentItem, SubWriterTools<TParent, TCollection, TNav> tools)
        {
            _parentItem = parentItem;
            _tools = tools;
            
            _castedExpression = Expression.Lambda<Func<TParent, IEnumerable<TNav>>>(_tools.NavExpression.Body, _tools.NavExpression.Parameters);
        }

        public async Task InitializeAsync()
        {
            // currently this is the only time this method is used.. perhaps this can be combined with CreateAndAttachItem ?
            await _parentItem.LoadAsync();
        }

        public IQueryable<TNav> GetAllItems()
        {
            return _parentItem.Query.SelectMany(_castedExpression);
        }

        public TNav CreateAndAttachItem()
        {
            ICollection<TNav> navCollection = GetNavCollection();

            var item = new TNav();

            _tools.RepoEvents?.OnCreating(item);
            navCollection.Add(item);

            return item;
        }

        public void MarkUpdated(TNav item)
        {
            _tools.RepoEvents?.OnUpdating(item);
        }

        public void MarkDeleted(TNav item)
        {
            ICollection<TNav> navCollection = GetNavCollection();

            _tools.RepoEvents?.OnDeleting(item);
            navCollection.Remove(item);
        }

        private ICollection<TNav> GetNavCollection()
        {
            TParent parentItem;
            IEnumerable<TNav> navEnumerable;

            try
            {
                parentItem = _parentItem.LoadedItem;
                navEnumerable = _tools.NavExpression.Compile().Invoke(parentItem);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Error getting navigation collection from parent item.", ex);
            }

            if (navEnumerable == null)
            {
                var newList = new List<TNav>();
                var castedList = newList as TCollection;
                Debug.Assert(castedList != null, "This should always cast. Is it not covariant?");

                _tools.Setter.SetNavItem(parentItem, castedList);
                return newList;
            }

            var navCollection = navEnumerable as ICollection<TNav>;
            if (navCollection == null)
                throw new NotSupportedException("Cannot add items to this navigation property because it does not implement ICollection.");

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