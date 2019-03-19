using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Data
{
    /// <summary>
    /// Interface to manage CRUD operations of engine items.
    /// </summary>
    public interface IEngineRepository<TItem>
        where TItem : class
    {
        [Obsolete("Only used in NavigationCollectionRepository. Possibly move that functionality.")]
        Task InitializeAsync();

        IQueryable<TItem> GetAllItems();

        TItem CreateAndAttachItem();

        void MarkUpdated(TItem item);

        void MarkDeleted(TItem item);
    }
}