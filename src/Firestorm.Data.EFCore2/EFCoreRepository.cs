using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.Data.EFCore2
{
    public class EFCoreRepository<TEntity> : IEngineRepository<TEntity>
        where TEntity : class, new()
    {
        private readonly DbSet<TEntity> _table;

        protected internal EFCoreRepository(DbSet<TEntity> table)
        {
            _table = table;
        }

        public TEntity CreateAndAttachItem()
        {
            TEntity entity = CreateUnattachedEntity();
            _table.Add(entity);
            return entity;
        }

        public Task InitializeAsync()
        {
            return Task.FromResult(false);
        }

        public IQueryable<TEntity> GetAllItems()
        {
            return _table.Where(AvailableToApiPredicate);
        }

        public void MarkDeleted(TEntity item)
        {
            _table.Remove(item);
        }

        protected virtual TEntity CreateUnattachedEntity()
        {
            return new TEntity();
        }

        protected virtual Expression<Func<TEntity, bool>> AvailableToApiPredicate
        {
            get { return entity => true; }
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return query.ForEachAsync(action);
        }
    }
}