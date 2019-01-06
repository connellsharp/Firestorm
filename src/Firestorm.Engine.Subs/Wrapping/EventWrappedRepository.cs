using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine.Subs.Wrapping
{
    /// <summary>
    /// Wraps and implements <see cref="IEngineRepository{TItem}"/> and calls methods on the given events object when repository methods are called.
    /// </summary>
    internal class EventWrappedRepository<TItem> : IEngineRepository<TItem>, ITransactionEvents
        where TItem : class
    {
        private readonly IEngineRepository<TItem> _repository;
        private readonly IDataChangeEvents<TItem> _events;
        private readonly List<TItem> _savableItems;

        internal EventWrappedRepository(IEngineRepository<TItem> repository, IDataChangeEvents<TItem> events)
        {
            _repository = repository;
            _events = events;
            _savableItems = new List<TItem>();
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
            _savableItems.Add(item);
            return item;
        }

        public void MarkUpdated(TItem item)
        {
            _events?.OnUpdating(item);
            _repository.MarkUpdated(item);
            _savableItems.Add(item);
        }

        public void MarkDeleted(TItem item)
        {
            var deleteResult = _events?.OnDeleting(item) ?? DeletingResult.Continue;
            
            if (deleteResult == DeletingResult.Continue)
                _repository.MarkDeleted(item);
            
            _savableItems.Add(item);
        }

        public async Task OnSavingAsync()
        {
            if (_events == null)
                return;

            foreach (TItem item in _savableItems)
                await _events.OnSavingAsync(item);
        }

        public async Task OnSavedAsync()
        {
            if (_events == null)
                return;

            foreach (TItem item in _savableItems)
                await _events.OnSavedAsync(item);
        }
    }
}