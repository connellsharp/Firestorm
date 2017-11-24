using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine;

namespace Firestorm.Stems.Fuel.Substems.Repositories
{
    /// <summary>
    /// An engine repository containing no items, but with the ability to create a new one.
    /// </summary>
    internal class CreateItemRepository<T> : IEngineRepository<T>
        where T : class, new()
    {
        private readonly T[] _emptyArray = { };

        public T CreateAndAttachItem()
        {
            return new T();
        }

        public Task InitializeAsync()
        {
            return Task.FromResult(false);
        }

        public IQueryable<T> GetAllItems()
        {
            return _emptyArray.AsQueryable();
        }

        public void MarkDeleted(T item)
        {
            throw new NotImplementedException("Not implemented deleting a created sub-item.");
        }

        public Task ForEachAsync<T1>(IQueryable<T1> query, Action<T1> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}