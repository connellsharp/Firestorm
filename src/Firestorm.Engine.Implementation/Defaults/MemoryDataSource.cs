using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Firestorm.Data;

namespace Firestorm.Engine.Defaults
{
    public class MemoryDataSource : IDataSource, IEnumerable
    {
        private readonly ConcurrentDictionary<Type, IList> _lists = new ConcurrentDictionary<Type, IList>();

        public IDataTransaction CreateTransaction()
        {
            return new TestTransaction();
        }

        public IEngineRepository<TItem> GetRepository<TItem>(IDataTransaction transaction)
            where TItem : class, new()
        {
            IList<TItem> list = GetList<TItem>();
            return new MemoryRepository<TItem>(list);
        }

        public void Add<TItem>(IEnumerable<TItem> items)
        {
            List<TItem> list = GetList<TItem>();
            list.AddRange(items);
        }

        private List<TEntity> GetList<TEntity>()
        {
            return (List<TEntity>)_lists.GetOrAdd(typeof(TEntity), t => new List<TEntity>());
        }

        public IEnumerator GetEnumerator()
        {
            return _lists.Values.GetEnumerator();
        }
    }
}