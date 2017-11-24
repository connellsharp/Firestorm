using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Data
{
    /// <summary>
    /// Interface to manage CRUD operations of engine items.
    /// </summary>
    public interface IEngineRepository<TItem> : IAsyncQueryer
        where TItem : class
    {
        Task InitializeAsync();

        IQueryable<TItem> GetAllItems();

        TItem CreateAndAttachItem();

        void MarkDeleted(TItem item);
    }
}