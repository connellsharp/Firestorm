using Firestorm.Data;
using Firestorm.Engine.Subs.Handlers;

namespace Firestorm.Engine.Subs.Repositories
{
    public static class RepositoryWrapperUtility
    {
        public static IEngineRepository<TItem> TryWrapEvents<TItem>(IEngineRepository<TItem> repository, IRepositoryEvents<TItem> events)
            where TItem : class
        {
            if (events != null && events.HasAnyEvent)
                return new RepositoryEventWrapper<TItem>(repository, events);

            return repository;
        }
    }
}