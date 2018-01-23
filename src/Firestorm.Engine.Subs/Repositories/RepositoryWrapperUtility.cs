using Firestorm.Data;
using Firestorm.Engine.Subs.Handlers;

namespace Firestorm.Engine.Subs.Repositories
{
    public static class RepositoryWrapperUtility
    {
        /* This was originally here for sub collections too, but they now accept an object containing the events and call them themselves.
           Now it is only used by the Fluent root repo. */

        /// <summary>
        /// If there are any events, wraps the repo in another implementation that triggers the events.
        /// Note: You do not need to wrap <see cref="NavigationItemRepository{TParent,TNav}"/> or <see cref="NavigationCollectionRepository{TParent,TCollection,TNav}"/>.
        /// </summary>
        public static IEngineRepository<TItem> TryWrapEvents<TItem>(IEngineRepository<TItem> repository, IRepositoryEvents<TItem> events)
            where TItem : class
        {
            if (events != null && events.HasAnyEvent)
                return new RepositoryEventWrapper<TItem>(repository, events);

            return repository;
        }
    }
}