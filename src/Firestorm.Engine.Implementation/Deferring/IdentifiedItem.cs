using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Data;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine.Deferring
{
    /// <summary>
    /// A single item's repository and identifier.
    /// </summary>
    internal class IdentifiedItem<TItem> : DeferredItemBase<TItem>
        where TItem : class
    {
        private readonly IEngineRepository<TItem> _repository;
        private readonly IIdentifierInfo<TItem> _identifierInfo;

        public IdentifiedItem(string identifier, IEngineRepository<TItem> repository, IIdentifierInfo<TItem> identifierInfo, IAsyncQueryer asyncQueryer)
            : base(identifier, asyncQueryer)
        {
            _repository = repository;
            _identifierInfo = identifierInfo;

            IQueryable<TItem> items = _repository.GetAllItems();
            Expression<Func<TItem, bool>> identifierPredicate = _identifierInfo.GetPredicate(identifier);
            Query = items.SingleDefferred(identifierPredicate);
        }

        protected override TItem CreateAttachAndSetItem()
        {
            if (!_identifierInfo.AllowsUpsert)
                return null;

            TItem item = _repository.CreateAndAttachItem();
            _identifierInfo.SetValue(item, Identifier);
            return item;
        }
    }
}