using System;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine;

namespace Firestorm.Stems.Roots.Derive
{
    internal class RootDataTransaction : IDataTransaction
    {
        private readonly Root _root;

        public RootDataTransaction(Root root)
        {
            _root = root;
        }

        public void StartTransaction()
        {
        }

        public Task SaveChangesAsync()
        {
            return _root.SaveChangesAsync();
        }

        public Task RollbackAsync()
        {
            throw new NotImplementedException("Not implemented rollback for Derived Roots.");
        }

        public void Dispose()
        {
            // this transaction doesn't actually get disposed by the RootCollectionCreator when the Root is disposed
            // because the line below would cause an infinite loop. Perhaps it shouldn't be here anyway?
            _root.Dispose();
        }
    }
}