using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using JetBrains.Annotations;

namespace Firestorm.Engine.Deferring
{
    /// <summary>
    /// A single item's repository and identifier. Contains methods to lazily fetch the item from the repository.
    /// </summary>
    public abstract class DeferredItemBase<TItem> : IDeferredItem<TItem>
        where TItem : class
    {
        private readonly IAsyncQueryer _queryer;
        private readonly SemaphoreSlimmer _semaphore = new SemaphoreSlimmer(0);
        private TItem _loadedItem;

        protected DeferredItemBase(string identifier, IAsyncQueryer queryer)
        {
            _queryer = queryer;
            LazyState = LazyState.NotLoaded;
            Identifier = identifier;
        }

        /// <summary>
        /// The string identifier used to describe the item.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// An <see cref="IQueryable{T}"/> that should return one or zero items.
        /// </summary>
        public IQueryableSingle<TItem> Query { get; protected set; }

        internal LazyState LazyState { get; private set; }

        public bool WasCreated
        {
            get { return LazyState == LazyState.Created; }
        }

        public TItem LoadedItem
        {
            get
            {
                if (LazyState == LazyState.NotLoaded)
                    throw new ItemNotLoadedException();

                return _loadedItem;
            }
            protected set
            {
                LazyState = LazyState.LoadedItem;
                _loadedItem = value;
            }
        }

        public async Task LoadAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                if (LazyState != LazyState.NotLoaded)
                    return; // already loaded

                _loadedItem = await ItemQueryHelper.SingleOrCreateAsync(Query, _queryer.ForEachAsync, TryCreateItem);

                if (LazyState != LazyState.Created)
                    LazyState = _loadedItem != null ? LazyState.LoadedItem : LazyState.NullValue;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private TItem TryCreateItem()
        {
            TItem item = CreateAttachAndSetItem();

            if (item == null)
                return ItemQueryHelper.ThrowNotFound<TItem>();

            LazyState = LazyState.Created;
            return item;
        }

        [CanBeNull]
        protected virtual TItem CreateAttachAndSetItem()
        {
            return null;
        }

        private class ItemNotLoadedException : InvalidOperationException
        {
            internal ItemNotLoadedException()
                : base("Referenced item was not loaded. Try calling LoadAsync first.")
            { }
        }
    }
}