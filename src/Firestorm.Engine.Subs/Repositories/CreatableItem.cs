using System.Linq;
using Firestorm.Data;
using Firestorm.Engine.Deferring;

namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// An item that can be created. 
    /// </summary>
    internal class CreatableItem<TItem> : DeferredItemBase<TItem>
        where TItem : class, new()
    {
        private readonly IEngineRepository<TItem> _navRepository;

        public CreatableItem(IEngineRepository<TItem> navRepository, IAsyncQueryer asyncQueryer)
            : base(null, asyncQueryer)
        {
            _navRepository = navRepository;

            Query = Enumerable.Empty<TItem>().SingleDefferred();
        }

        protected override TItem CreateAttachAndSetItem()
        {
            return _navRepository.CreateAndAttachItem();
        }
    }
}