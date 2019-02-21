using System;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine.Defaults
{
    /// <summary>
    /// A dummy <see cref="IDataTransaction"/> implementation that does nothing.
    /// </summary>
    public class VoidTransaction : IDataTransaction
    {
        public void StartTransaction()
        {
            // Does nothing
        }

        public Task SaveChangesAsync()
        {
            return Task.FromResult(true);
        }

        public Task RollbackAsync()
        {
            throw new NotImplementedException("Not implemented rollback for Test transaction.");
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}