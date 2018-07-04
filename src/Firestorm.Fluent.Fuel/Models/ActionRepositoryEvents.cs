using System;
using Firestorm.Engine.Subs;

namespace Firestorm.Fluent.Fuel.Models
{
    /// <summary>
    /// An implementation of <see cref="IRepositoryEvents{TItem}"/> where all events are <see cref="Action"/> delegate properties.
    /// </summary>
    internal class ActionRepositoryEvents<T> : IRepositoryEvents<T>
    {
        public void OnCreating(T item)
        {
            OnCreatingAction?.Invoke(item);
        }

        public void OnDeleting(T item)
        {
            OnDeletingAction?.Invoke(item);
        }

        public Action<T> OnCreatingAction { get; set; }

        public Action<T> OnDeletingAction { get; set; }

        public bool HasAnyEvent
        {
            get { return OnCreatingAction != null || OnDeletingAction != null; }
        }
    }
}