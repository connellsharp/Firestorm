using Firestorm.Data;
using JetBrains.Annotations;

namespace Firestorm.Engine.Subs.Wrapping
{
    /// <summary>
    /// If there are any events, wraps the repo in another implementation that triggers the events.
    /// </summary>
    public class DataEventWrapper<TItem>
        where TItem : class
    {
        /* This was originally here for sub collections too, but they now accept an object containing the events and call them themselves.
           Now it is only used by the Fluent root repo. */

        public IDataTransaction Transaction { get; private set; }

        public IEngineRepository<TItem> Repository { get; private set; }

        public DataEventWrapper(IDataTransaction transaction, IEngineRepository<TItem> repository)
        {
            Transaction = transaction;
            Repository = repository;
        }

        public void TryWrapEvents([CanBeNull] IDataChangeEvents<TItem> events)
        {
            if (events == null || !events.HasAnyEvent)
                return;

            var wrappedRepo = new EventWrappedRepository<TItem>(Repository, events);

            Repository = wrappedRepo;
            Transaction = new EventWrappedTransaction(Transaction, wrappedRepo);
        }
    }
}