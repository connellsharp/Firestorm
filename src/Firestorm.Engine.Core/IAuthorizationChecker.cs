using System.Linq;

namespace Firestorm.Engine
{
    /// <summary>
    /// Checks different levels for authorization to the requested resource.
    /// </summary>
    public interface IAuthorizationChecker<TItem>
        where TItem : class
    {
        IQueryable<TItem> ApplyFilter(IQueryable<TItem> items);

        bool CanGetItem(IDeferredItem<TItem> item);

        bool CanAddItem();

        bool CanEditItem(IDeferredItem<TItem> item);

        bool CanDeleteItem(IDeferredItem<TItem> item);

        bool CanGetField(IDeferredItem<TItem> item, INamedField<TItem> field); // TODO: call on select collection

        bool CanEditField(IDeferredItem<TItem> item, INamedField<TItem> field);
    }
}