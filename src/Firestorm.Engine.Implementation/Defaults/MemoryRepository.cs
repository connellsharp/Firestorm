using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine.Defaults
{
    public class MemoryRepository<TItem> : IEngineRepository<TItem>
        where TItem : class, new()
    {
        private readonly ICollection<TItem> _collection;

        public MemoryRepository(ICollection<TItem> collection)
        {
            _collection = collection;
        }

        public Task InitializeAsync()
        {
            return Task.FromResult(false);
        }

        public IQueryable<TItem> GetAllItems()
        {
            return _collection.AsQueryable();
        }

        public void MarkDeleted(TItem item)
        {
            _collection.Remove(item);
        }

        public TItem CreateAndAttachItem()
        {
            var item = new TItem();
            _collection.Add(item);
            return item;
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}