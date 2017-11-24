using System;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine.Defaults
{
    /// <summary>
    /// A dummy <see cref="IDataTransaction"/> implementation that does nothing.
    /// </summary>
    public class TestTransaction : IDataTransaction
    {
        public void StartTransaction()
        {
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
        }
    }
}