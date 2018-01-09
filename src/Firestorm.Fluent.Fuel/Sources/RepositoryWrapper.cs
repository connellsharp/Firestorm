using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Fluent.Fuel.Sources
{
    internal class RepositoryWrapper<TItem>: IEngineRepository<TItem>
        where TItem : class
    {
        private readonly IEngineRepository<TItem> _repository;
        private readonly Action<TItem> _onCreating;

        public RepositoryWrapper(IEngineRepository<TItem> repository, Action<TItem> onCreating)
        {
            _repository = repository;
            _onCreating = onCreating;
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
            _onCreating?.Invoke(item);
            return item;
        }

        public void MarkDeleted(TItem item)
        {
            _repository.MarkDeleted(item);
        }
    }
}