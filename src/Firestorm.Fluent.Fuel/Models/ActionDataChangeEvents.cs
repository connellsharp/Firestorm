using System;
using System.Threading.Tasks;
using Firestorm.Engine.Subs;

namespace Firestorm.Fluent.Fuel.Models
{
    /// <summary>
    /// An implementation of <see cref="IDataChangeEvents{TItem}"/> where all events are <see cref="Action"/> delegate properties.
    /// </summary>
    internal class ActionDataChangeEvents<T> : IDataChangeEvents<T>
    {
        public void OnCreating(T newItem)
        {
            OnCreatingAction?.Invoke(newItem);
        }

        public void OnUpdating(T item)
        {
            // TODO no updating event in fluent yet
        }

        public DeletingResult OnDeleting(T item)
        {
            OnDeletingAction?.Invoke(item);
            
            // TODO no soft delete in fluent yet
            return DeletingResult.Continue;
        }

        public Task OnSavingAsync(T item)
        {
            return Task.FromResult(false);
        }

        public Task OnSavedAsync(T item)
        {
            return Task.FromResult(false);
        }

        public Action<T> OnCreatingAction { get; set; }

        public Action<T> OnDeletingAction { get; set; }

        public bool HasAnyEvent
        {
            get { return OnCreatingAction != null || OnDeletingAction != null; }
        }
    }
}