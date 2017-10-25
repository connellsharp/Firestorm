using System;
using Firestorm.Engine.Data;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.Engine.EFCore2
{
    public class EFCoreDataSource<TDatabase> : IDataSource
        where TDatabase : DbContext
    {
        private readonly IServiceProvider _servicerProvider;

        public EFCoreDataSource(IServiceProvider servicerProvider)
        {
            _servicerProvider = servicerProvider;
        }

        public IDataTransaction CreateTransaction()
        {
            var database = (TDatabase)_servicerProvider.GetService(typeof(TDatabase));
            return new EFCoreDataTransaction<TDatabase>(database);
        }

        public IEngineRepository<TEntity> GetRepository<TEntity>(IDataTransaction transaction)
            where TEntity : class, new()
        {
            if (!(transaction is EFCoreDataTransaction<TDatabase> entitiesTransaction))
                throw new ArgumentException("Entity Framework Core data source was given a transaction for the wrong data source type or context.");

            TDatabase database = entitiesTransaction.DbContext;
            DbSet<TEntity> repo = database.Set<TEntity>();
            return new EFCoreRepository<TEntity>(repo);
        }
    }
}