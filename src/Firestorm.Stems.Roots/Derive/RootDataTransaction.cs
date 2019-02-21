using System;
using System.Threading.Tasks;
using Firestorm.Data;

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
            // Root itself behaves like the transaction
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
            // this transaction is actually disposed by the Stem and Root when they are disposed.
            // therefore the line below would cause an infinite loop.
            
            //_root.Dispose();
        }
    }
}