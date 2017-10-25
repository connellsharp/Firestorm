using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm.Stems
{
    /// <summary>
    /// The base for Firestorm Stems: a neat, but not a necessary way to write Firestorm Web APIs.
    /// </summary>
    public abstract class Stem<TItem> : Stem
        where TItem : class
    {
        [CanBeNull]
        public virtual Expression<Func<TItem, ItemPermission>> PermissionExpression
        {
            get { return null; }
        }

        public virtual void OnItemCreated(TItem newItem)
        {
        }

        public virtual bool MarkDeleted(TItem item)
        {
            return false;
        }
    }

    /// <summary>
    /// The internal, typeless base for Firestorm Stems.
    /// </summary>
    public abstract class Stem : IAxis
    {
        /// <summary>
        /// Internal constructor prevents creating weakly-typed Stems.
        /// </summary>
        internal Stem()
        { }

        public IRestUser User { get; private set; }

        public IStemConfiguration Configuration { get; private set; }

        public void SetParent(IAxis parent)
        {
            User = parent.User;
            Configuration = parent.Configuration;
            parent.OnDispose += (sender, args) => Dispose();
        }
        
        public virtual bool CanAddItem()
        {
            return false; // TODO: good default?
        }

        public event EventHandler OnDispose;

        public virtual void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}