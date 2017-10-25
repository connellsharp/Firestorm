using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine;

namespace Firestorm.Stems.Roots.Derive
{
    /// <summary>
    /// A hybrid root that extracts necessary properties from a given <see cref="IDataTransaction"/> and <see cref="IEngineRepository{TItem}"/>
    /// , only for new implementations of those to be created later on.
    /// </summary>
    public abstract class EngineRoot<TItem> : Root<TItem>
        where TItem : class
    {
        protected abstract IDataTransaction DataTransaction { get; }

        protected abstract IEngineRepository<TItem> Repository { get; }

        public sealed override Task SaveChangesAsync()
        {
            return DataTransaction.SaveChangesAsync();
        }

        public sealed override IQueryable<TItem> GetAllItems()
        {
            return Repository.GetAllItems();
        }

        public sealed override TItem CreateAndAttachItem()
        {
            return Repository.CreateAndAttachItem();
        }

        public sealed override void MarkDeleted(TItem item)
        {
            Repository.MarkDeleted(item);
        }
    }
}