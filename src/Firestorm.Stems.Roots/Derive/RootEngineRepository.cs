using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Stems.Roots.Combined;

namespace Firestorm.Stems.Roots.Derive
{
    internal class RootEngineRepository<TItem> : IEngineRepository<TItem>
        where TItem : class
    {
        private readonly Root<TItem> _root;

        public RootEngineRepository(Root<TItem> root)
        {
            _root = root;
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
            return newItem;
        }

        public void MarkDeleted(TItem item)
        {
            _root.MarkDeleted(item);
        }
    }
}