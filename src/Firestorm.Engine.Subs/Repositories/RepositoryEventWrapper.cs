using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Subs.Handlers;

namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// Wraps and implements <see cref="IEngineRepository{TItem}"/> and calls methods on the given events object when repository methods are called.
    /// Note: You do not need to wrap <see cref="NavigationItemRepository{TParent,TNav}"/> or <see cref="NavigationCollectionRepository{TParent,TCollection,TNav}"/>.
    /// </summary>
    internal class RepositoryEventWrapper<TItem>: IEngineRepository<TItem>
        where TItem : class
    {
        private readonly IEngineRepository<TItem> _repository;
        private readonly IRepositoryEvents<TItem> _events;

        internal RepositoryEventWrapper(IEngineRepository<TItem> repository, IRepositoryEvents<TItem> events)
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
}