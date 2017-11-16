using System;
using Firestorm.Engine.Data;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.Engine.EFCore2
{
    public class EFCoreDataSource<TDatabase> : IDataSource
        where TDatabase : DbContext
    {
        private readonly Func<TDatabase> _getServiceFunc;

        public EFCoreDataSource(Func<TDatabase> getServiceFunc)
        {
            _getServiceFunc = getServiceFunc;
        }

        public EFCoreDataSource(IServiceProvider serviceProvider)
        {
            _getServiceFunc = () => (TDatabase)serviceProvider.GetService(typeof(TDatabase));
        }

        public IDataTransaction CreateTransaction()
        {
            TDatabase database = _getServiceFunc();
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