using Firestorm.Data;
using JetBrains.Annotations;

namespace Firestorm.Engine.Subs.Wrapping
{
    /// <summary>
    /// If there are any events, decorates the transaction and repo with implementations that triggers the events.
    /// </summary>
    public class EventTriggeringDataContext<TItem> : IDataContext<TItem>
        where TItem : class
    {
        /* Old comment from DataEventWrapper. TODO refactor ?:
           This was originally here for sub collections too, but they now accept an object containing the events and call them themselves.
           Now it is only used by the Fluent root repo. */
        
        public EventTriggeringDataContext(IDataContext<TItem> underlyingContext, [CanBeNull] IDataChangeEvents<TItem> events)
        {
            if (events == null || !events.HasAnyEvent)
            {
                var wrappedRepo = new EventWrappedRepository<TItem>(underlyingContext.Repository, events);

                Transaction = new EventWrappedTransaction(underlyingContext.Transaction, wrappedRepo);
                Repository = wrappedRepo;
            }
            else
            {
                Transaction = underlyingContext.Transaction;
                Repository = underlyingContext.Repository;
            }

            AsyncQueryer = underlyingContext.AsyncQueryer;
        }
        
        public IDataTransaction Transaction { get; }
        public IEngineRepository<TItem> Repository { get; }
        public IAsyncQueryer AsyncQueryer { get; }
    }
}