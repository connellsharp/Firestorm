using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.EntityFramework6
{
    /// <summary>
    /// Entity Framework transaction object that creates and disposes of the database context.
    /// </summary>
    public class EntitiesDataTransaction<TDatabase> : IDataTransaction
        where TDatabase : DbContext, new()
    {
        public EntitiesDataTransaction()
        {
            DbContext = new TDatabase();
        }

        public TDatabase DbContext { get; }

        public void StartTransaction()
        {
            //_database = new TDatabase();
            // TODO
        }

        public async Task SaveChangesAsync()
        {
            OnSavingChanges();
            await DbContext.SaveChangesAsync();
            OnSavedChanges();
        }

        protected virtual void OnSavedChanges()
        {
        }

        protected virtual void OnSavingChanges()
        {
        }

        public Task RollbackAsync()
        {
            throw new NotImplementedException("Not implemented rollback for Entity Framework.");
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
