using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Engine.Defaults
{
    public abstract class MemoryRepository<TItem> : IEngineRepository<TItem>
        where TItem : class, new()
    {
        private List<TItem> _list;

        public MemoryRepository()
        {
            Initialize();
        }

        public void Initialize() // TODO: repo vs transaction
        {
            if (_list == null)
                _list = new List<TItem>(LoadInitialRepository());
        }

        protected abstract IEnumerable<TItem> LoadInitialRepository();

        public Task InitializeAsync()
        {
            return Task.FromResult(false);
        }

        public IQueryable<TItem> GetAllItems()
        {
            return _list.AsQueryable();
        }

        public void MarkDeleted(TItem item)
        {
            _list.Remove(item);
        }

        public TItem CreateAndAttachItem()
        {
            var item = new TItem();
            _list.Add(item);
            return item;
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}