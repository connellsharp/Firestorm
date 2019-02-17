using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Deferring;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Reflectious;

namespace Firestorm.Stems.Fuel
{
    internal class StemAuthorizationChecker<TItem> : IAuthorizationChecker<TItem>
        where TItem : class
    {
        private readonly Stem<TItem> _stem;
        private readonly EngineImplementations<TItem> _implementations;

        private static readonly IAuthorizationChecker<TItem> AllowAllChecker = new AllowAllAuthorizationChecker<TItem>();

        public StemAuthorizationChecker(Stem<TItem> stem, EngineImplementations<TItem> implementations)
        {
            _stem = stem;
            _implementations = implementations;
        }

        public virtual IQueryable<TItem> ApplyFilter(IQueryable<TItem> items)
        {
            var permissionPredicate = GetPermissionPredicate(ItemPermission.Read);
            if (permissionPredicate != null)
                return items.Where(permissionPredicate);

            return AllowAllChecker.ApplyFilter(items);
        }

        public virtual bool CanGetItem(IDeferredItem<TItem> item)
        {
            Expression<Func<TItem, bool>> permissionPredicate = GetPermissionPredicate(ItemPermission.Read);
            if (permissionPredicate != null)
                return item.Query.Any(permissionPredicate);

            return AllowAllChecker.CanGetItem(item);
        }

        public virtual bool CanAddItem()
        {
            return _stem.CanAddItem();
        }

        public virtual bool CanEditItem(IDeferredItem<TItem> item)
        {
            var permissionPredicate = GetPermissionPredicate(ItemPermission.Write);
            if (permissionPredicate != null)
                return item.Query.Any(permissionPredicate);

            return AllowAllChecker.CanEditItem(item);
        }

        public virtual bool CanDeleteItem(IDeferredItem<TItem> item)
        {
            var permissionPredicate = GetPermissionPredicate(ItemPermission.Delete);
            if (permissionPredicate != null)
                return item.Query.Any(permissionPredicate);

            return AllowAllChecker.CanDeleteItem(item);
        }

        public virtual bool CanGetField(IDeferredItem<TItem> item, INamedField<TItem> field)
        {
            Func<IRestUser, bool> authorizePredicate = GetFieldAuthorizePredicate(field.Name);

            if (authorizePredicate != null)
                return authorizePredicate.Invoke(_stem.User); // TODO: item

            return AllowAllChecker.CanGetField(item, field);
        }

        public virtual bool CanEditField(IDeferredItem<TItem> item, INamedField<TItem> field)
        {
            Func<IRestUser, bool> authorizePredicate = GetFieldAuthorizePredicate(field.Name);

            if (authorizePredicate != null)
                return authorizePredicate.Invoke(_stem.User); // TODO: item

            return AllowAllChecker.CanEditField(item, field);
        }

        private Func<IRestUser, bool> GetFieldAuthorizePredicate(string fieldName)
        {
            return _implementations.AuthorizePredicates.ContainsKey(fieldName) ? _implementations.AuthorizePredicates[fieldName] : null;
        }

        private Expression<Func<TItem, bool>> GetPermissionPredicate(ItemPermission permission)
        {
            return _stem.PermissionExpression?.Chain(p => (p & permission) != 0);
            //return _stem.PermissionExpression?.Chain(p => p.HasFlag(permission)); // supported in EF6.1 apparently?
        }
    }
}