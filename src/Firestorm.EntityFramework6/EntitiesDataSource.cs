using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Firestorm.Data;

namespace Firestorm.EntityFramework6
{
    public class EntitiesDataSource<TDbContext> : IDataSource
        where TDbContext : DbContext, new()
    {
        private readonly IAsyncQueryer _asyncQueryer;

        public EntitiesDataSource()
        {
            _asyncQueryer = new EntitiesAsyncQueryer();
        }

        public IEnumerable<Type> FindEntityTypes()
        {
            using (var context = new TDbContext())
            {
                return IterateEntityClrTypes(context).ToList();
            }
        }

        public IDataContext<TEntity> CreateContext<TEntity>() where TEntity : class, new()
        {
            var dbContext = new TDbContext();
            DbSet<TEntity> dbSet = dbContext.Set<TEntity>();

            return new DataContext<TEntity>
            {
                Transaction = new EntitiesDataTransaction<TDbContext>(dbContext),
                Repository = new EntitiesRepository<TEntity>(dbSet),
                AsyncQueryer = _asyncQueryer
            };
        }

        /// <remarks>
        /// From https://stackoverflow.com/a/18950910
        /// </remarks>
        private static IEnumerable<Type> IterateEntityClrTypes(TDbContext dbContext)
        {
            var objectItemCollection = (ObjectItemCollection) ((IObjectContextAdapter) dbContext)
                .ObjectContext.MetadataWorkspace.GetItemCollection(DataSpace.OSpace);

            foreach (var entityType in objectItemCollection.GetItems<EntityType>())
            {
                yield return objectItemCollection.GetClrType(entityType);
            }
        }
    }
}