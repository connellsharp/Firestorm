using System;
using System.Collections.Generic;
using Firestorm.Data;

namespace Firestorm.Stems.Roots.Derive
{
    public class RootDataSource : IDataSource
    {
        private readonly Root _root;

        public RootDataSource(Root root)
        {
            _root = root;
        }

        public IEnumerable<Type> FindEntityTypes()
        {
            throw new NotSupportedException("Cannot find data types for Roots.");
        }

        public IDataContext<TEntity> CreateContext<TEntity>() where TEntity : class, new()
        {
            return new DataContext<TEntity>
            {
                Transaction = new RootDataTransaction(_root),
                Repository = new RootEngineRepository<TEntity>((Root<TEntity>) _root),
                AsyncQueryer = new RootAsyncQueryer(_root)
            };
        }
    }
}