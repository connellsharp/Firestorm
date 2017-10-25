using System.Linq;
using Firestorm.Engine;

namespace Firestorm.Stems.Fuel.Substems.Repositories
{
    /// <summary>
    /// An item that can be created. 
    /// </summary>
    internal class CreatableItem<TItem> : DeferredItemBase<TItem>
        where TItem : class, new()
    {
        private readonly IEngineRepository<TItem> _navRepository;

        public CreatableItem(IEngineRepository<TItem> navRepository)
            : base(null, navRepository)
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