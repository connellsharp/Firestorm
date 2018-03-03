using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Firestorm.Data;

namespace Firestorm.EntityFramework6
{
    public class EntitiesDataSource<TDbContext> : IDiscoverableDataSource
        where TDbContext : DbContext, new()
    {
        public IDataTransaction CreateTransaction()
        {
            return new EntitiesDataTransaction<TDbContext>();
        }

        public IEngineRepository<TEntity> GetRepository<TEntity>(IDataTransaction transaction)
            where TEntity : class, new()
        {
            if(!(transaction is EntitiesDataTransaction<TDbContext> entitiesTransaction))
                throw new ArgumentException("Entity Framework data source was given a transaction for the wrong data source type or context.");

            TDbContext database = entitiesTransaction.DbContext;
            DbSet<TEntity> repo = database.Set<TEntity>();
            return new EntitiesRepository<TEntity>(repo);
        }

        public IEnumerable<Type> FindRespositoryTypes()
        {
            using (var context = new TDbContext())
            {
                return IterateEntityClrTypes(context).ToList();
            }
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