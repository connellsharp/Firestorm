using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Firestorm.Data;

namespace Firestorm.Engine.Defaults
{
    public class MemoryDataSource : IDataSource, IEnumerable<IList>
    {
        private readonly ConcurrentDictionary<Type, IList> _lists = new ConcurrentDictionary<Type, IList>();

        public void Add<TItem>(IEnumerable<TItem> items)
        {
            List<TItem> list = GetList<TItem>();
            list.AddRange(items);
        }

        private List<TEntity> GetList<TEntity>()
        {
            return (List<TEntity>)_lists.GetOrAdd(typeof(TEntity), t => new List<TEntity>());
        }

        public IEnumerable<Type> FindEntityTypes()
        {
            return _lists.Keys;
        }

        public IDataContext<TEntity> CreateContext<TEntity>() where TEntity : class, new()
        {
            IList<TEntity> list = GetList<TEntity>();
            
            return new DataContext<TEntity>
            {
                Transaction = new VoidTransaction(),
                Repository = new MemoryRepository<TEntity>(list),
                AsyncQueryer = new MemoryAsyncQueryer()
            };
        }

        IEnumerator<IList> IEnumerable<IList>.GetEnumerator()
        {
            return _lists.Values.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _lists.Values.GetEnumerator();
        }
    }
}