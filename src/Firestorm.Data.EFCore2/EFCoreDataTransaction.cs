using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.Data.EFCore2
{
    /// <summary>
    /// EF Core transaction using a given database context.
    /// Context is not disposed with this object.
    /// </summary>
    public class EFCoreDataTransaction<TDatabase> : IDataTransaction
        where TDatabase : DbContext
    {
        public EFCoreDataTransaction(TDatabase database)
        {
            DbContext = database;
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
            throw new NotImplementedException("Not implemented rollback for EF Core.");
        }

        public void Dispose()
        {
            // passed in to constructor.
            // EF Core service provider takes care of disposing the context
            //DbContext.Dispose();
        }
    }
}
