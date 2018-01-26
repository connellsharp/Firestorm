using System;
using System.Data.Entity;
using Firestorm.Data;

namespace Firestorm.EntityFramework6
{
    public class EntitiesDataSource<TDatabase> : IDataSource
        where TDatabase : DbContext, new()
    {
        public IDataTransaction CreateTransaction()
        {
            return new EntitiesDataTransaction<TDatabase>();
        }

        public IEngineRepository<TEntity> GetRepository<TEntity>(IDataTransaction transaction)
            where TEntity : class, new()
        {
            if(!(transaction is EntitiesDataTransaction<TDatabase> entitiesTransaction))
                throw new ArgumentException("Entity Framework data source was given a transaction for the wrong data source type or context.");

            TDatabase database = entitiesTransaction.DbContext;
            DbSet<TEntity> repo = database.Set<TEntity>();
            return new EntitiesRepository<TEntity>(repo);
        }
    }
}