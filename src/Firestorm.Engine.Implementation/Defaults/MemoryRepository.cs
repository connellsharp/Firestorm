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
        private readonly IList<TItem> _list;

        public MemoryRepository(IList<TItem> items)
        {
            _list = items;
        }

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