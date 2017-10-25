using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine;

namespace Firestorm.Stems.Roots.Derive
{
    internal class RootEngineRepository<TItem> : IEngineRepository<TItem>
        where TItem : class
    {
        private readonly Root<TItem> _root;
        private readonly Stem<TItem> _stem;

        public RootEngineRepository(Root<TItem> root, Stem<TItem> stem)
        {
            _root = root;
            _stem = stem;
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return _root.ForEachAsync(query, action);
        }

        public Task InitializeAsync()
        {
            return Task.FromResult(false);
        }

        public IQueryable<TItem> GetAllItems()
        {
            return _root.GetAllItems();
        }

        public TItem CreateAndAttachItem()
        {
            TItem newItem = _root.CreateAndAttachItem();
            if (newItem == null)
                return null;

            _stem.OnItemCreated(newItem);
            
            return newItem;
        }

        public void MarkDeleted(TItem item)
        {
            if (!_stem.MarkDeleted(item))
                _root.MarkDeleted(item);
        }
    }
}