using System.Linq;

namespace Firestorm.Engine.Additives.Authorization
{
    public class AllowAllAuthorizationChecker<TItem> : IAuthorizationChecker<TItem>
        where TItem : class
    {
        public IQueryable<TItem> ApplyFilter(IQueryable<TItem> items)
        {
            return items;
        }

        public bool CanGetItem(IDeferredItem<TItem> item)
        {
            return true;
        }

        public bool CanAddItem()
        {
            return true;
        }

        public bool CanEditItem(IDeferredItem<TItem> item)
        {
            return true;
        }

        public bool CanDeleteItem(IDeferredItem<TItem> item)
        {
            return true;
        }

        public bool CanGetField(IDeferredItem<TItem> item, INamedField<TItem> field)
        {
            return true;
        }

        public bool CanEditField(IDeferredItem<TItem> item, INamedField<TItem> field)
        {
            return true;
        }
    }
}