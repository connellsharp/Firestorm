using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Data;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2
{
    public class EFCoreDataSource<TDbContext> : IDiscoverableDataSource
        where TDbContext : DbContext
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;

        public EFCoreDataSource(IDbContextFactory<TDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IDataTransaction CreateTransaction()
        {
            TDbContext database = _dbContextFactory.Create();
            return new EFCoreDataTransaction<TDbContext>(database);
        }

        public IEngineRepository<TEntity> GetRepository<TEntity>(IDataTransaction transaction)
            where TEntity : class, new()
        {
            if (!(transaction is EFCoreDataTransaction<TDbContext> entitiesTransaction))
                throw new ArgumentException("Entity Framework Core data source was given a transaction for the wrong data source type or context.");

            TDbContext database = entitiesTransaction.DbContext;
            DbSet<TEntity> repo = database.Set<TEntity>();
            return new EFCoreRepository<TEntity>(repo);
        }

        public IEnumerable<Type> FindRepositoryTypes()
        {
            using (TDbContext database = _dbContextFactory.Create())
            {
                return database.Model.GetEntityTypes().Select(t => t.ClrType).ToList();
            }
        }
    }
}