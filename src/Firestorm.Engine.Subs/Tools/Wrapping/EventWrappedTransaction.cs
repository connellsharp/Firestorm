using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine.Subs.Wrapping
{
    public class EventWrappedTransaction : IDataTransaction
    {
        private readonly IDataTransaction _transaction;
        private readonly ITransactionEvents _events;

        internal EventWrappedTransaction(IDataTransaction transaction, ITransactionEvents events)
        {
            _transaction = transaction;
            _events = events;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void StartTransaction()
        {
            _transaction.StartTransaction();
        }

        public async Task SaveChangesAsync()
        {
            await _events.OnSavingAsync();
            await _transaction.SaveChangesAsync();
            await _events.OnSavedAsync();
        }

        public Task RollbackAsync()
        {
            return _transaction.RollbackAsync();
        }
    }
}