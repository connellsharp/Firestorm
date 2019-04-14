using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Data;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2
{
    public class EFCoreDataSource<TDbContext> : IDataSource
        where TDbContext : DbContext
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;
        private readonly EFCoreAsyncQueryer _asyncQueryer;

        internal EFCoreDataSource(IDbContextFactory<TDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _asyncQueryer = new EFCoreAsyncQueryer();
        }

        public IEnumerable<Type> FindEntityTypes()
        {
            using (TDbContext database = _dbContextFactory.Create())
            {
                return database.Model.GetEntityTypes().Select(t => t.ClrType).ToList();
            }
        }

        public IDataContext<TEntity> CreateContext<TEntity>() 
            where TEntity : class, new()
        {
            TDbContext database = _dbContextFactory.Create();            
            DbSet<TEntity> dbSet = database.Set<TEntity>();

            return new DataContext<TEntity>
            {
                Transaction = new EFCoreDataTransaction<TDbContext>(database),
                Repository = new EFCoreRepository<TEntity>(dbSet),
                AsyncQueryer = _asyncQueryer
            };
        }
    }
}