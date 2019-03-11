using Firestorm.Data;
using Firestorm.Engine.Deferring;

namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// Can be used as a <see cref="IDeferredItem{TItem}"/> instance when a repositoryWithSingleItem only contains a single item.
    /// </summary>
    internal class RepositoryDeferredItem<TItem> : DeferredItemBase<TItem>
        where TItem : class
    {
        public RepositoryDeferredItem(IEngineRepository<TItem> repositoryWithSingleItem, IAsyncQueryer asyncQueryer)
            : base(null, asyncQueryer)
        {
            RepositoryWithSingleItem = repositoryWithSingleItem;
            Query = repositoryWithSingleItem.GetAllItems().SingleDefferred();
        }

        public IEngineRepository<TItem> RepositoryWithSingleItem { get; }
    }
}