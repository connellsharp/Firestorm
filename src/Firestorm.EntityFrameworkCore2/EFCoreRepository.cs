using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Firestorm.Data;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2
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
            var entity = new TEntity();
            AddEmptyCollections(entity);
            return entity;
        }

        private static void AddEmptyCollections(TEntity entity)
        {
            // EF Core doesn't create empty ICollection objects for the navigation properties, which causes errors in NavigationCollectionRepository.GetNavCollection.
            // This method adds all the empty collections. I'm not sure if Lazy-loading them is better, or if EF can add these itself when the entity is attached?

            foreach (PropertyInfo property in typeof(TEntity).GetProperties())
            {
                if (!EnumerableTypeUtility.IsEnumerable(property.PropertyType))
                    continue;

                if (property.GetValue(entity) != null)
                    continue;

                var itemType = EnumerableTypeUtility.GetItemType(property.PropertyType);
                var newList = EnumerableTypeUtility.CreateList(itemType);

                property.SetValue(entity, newList);
            }
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