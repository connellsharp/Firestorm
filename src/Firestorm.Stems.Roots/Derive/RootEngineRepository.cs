using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Stems.Roots.Derive
{
    internal class RootEngineRepository<TItem> : IEngineRepository<TItem>
        where TItem : class
    {
        private readonly Root<TItem> _root;

        public RootEngineRepository(Root<TItem> root)
        {
            _root = root;
        }

        public Task InitializeAsync()
        {
            return Task.FromResult(false);
        }

        public IQueryable<TItem> GetAllItems()
        {
            return _root.GetAllItems();
        }

        public TItem CreateAndAttachItem()
        {
            TItem newItem = _root.CreateAndAttachItem();
            return newItem;
        }

        public void MarkUpdated(TItem item)
        {
            _root.MarkUpdated(item);
        }

        public void MarkDeleted(TItem item)
        {
            _root.MarkDeleted(item);
        }
    }
}