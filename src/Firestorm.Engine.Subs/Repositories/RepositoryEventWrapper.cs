using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Subs.Handlers;

namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// Wraps and implements <see cref="IEngineRepository{TItem}"/> and calls methods on the given events object when repository methods are called.
    /// </summary>
    public class RepositoryEventWrapper<TItem>: IEngineRepository<TItem>
        where TItem : class
    {
        private readonly IEngineRepository<TItem> _repository;
        private readonly IRepositoryEvents<TItem> _events;

        public RepositoryEventWrapper(IEngineRepository<TItem> repository, IRepositoryEvents<TItem> events)
        {
            _repository = repository;
            _events = events;
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return _repository.ForEachAsync(query, action);
        }

        public Task InitializeAsync()
        {
            return _repository.InitializeAsync();
        }

        public IQueryable<TItem> GetAllItems()
        {
            return _repository.GetAllItems();
        }

        public TItem CreateAndAttachItem()
        {
            var item = _repository.CreateAndAttachItem();
            _events?.OnCreating(item);
            return item;
        }

        public void MarkDeleted(TItem item)
        {
            _repository.MarkDeleted(item);
        }
    }

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