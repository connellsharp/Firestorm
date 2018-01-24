using System;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Handlers;

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

        public Action<T> OnCreatingAction { get; set; }

        public bool HasAnyEvent
        {
            get { return OnCreatingAction != null; }
        }
    }
}