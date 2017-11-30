using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// An engine repository containing a single entity that has already been loaded into memory.
    /// </summary>
    internal class LoadedItemRepository<T> : IEngineRepository<T>
        where T : class
    {
        private readonly T _item;

        public LoadedItemRepository(T item)
        {
            _item = item;
        }

        public Task InitializeAsync()
        {
            return Task.FromResult(false);
        }

        public T CreateAndAttachItem()
        {
            const string msg = "Shouldn't get here because GetAllItems always returns some values.";
            Debug.Fail(msg);
            throw new NotImplementedException(msg);
        }

        public IQueryable<T> GetAllItems()
        {
            return new[] { _item }.AsQueryable();
        }

        public void MarkDeleted(T item)
        {
            throw new NotImplementedException("Not implemented deleting a loaded sub-item.");
        }

        public Task ForEachAsync<T1>(IQueryable<T1> query, Action<T1> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}