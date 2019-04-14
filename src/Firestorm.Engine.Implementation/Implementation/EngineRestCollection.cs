using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Identifiers;
using Firestorm.Engine.Queryable;
using JetBrains.Annotations;

namespace Firestorm.Engine
{
    [UsedImplicitly]
    public class EngineRestCollection<TItem> : IRestCollection
        where TItem : class
    {
        private readonly IEngineContext<TItem> _context;

        public EngineRestCollection([NotNull] IEngineContext<TItem> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<RestCollectionData> QueryDataAsync(IRestCollectionQuery query)
        {
            await _context.Data.Repository.InitializeAsync();
            
            var fieldAuth = new FieldAuthChecker<TItem>(_context.Fields, _context.AuthorizationChecker, null);
            IEnumerable<string> fields = fieldAuth.GetOrEnsureFields(query?.SelectFields, 1);
            var selector = new QueryableFieldSelector<TItem>(_context.Fields.GetReaders(fields));

            var queryBuilder = new ContextQueryBuilder<TItem>(_context, query);
            IQueryable<TItem> items = queryBuilder.BuildQueryable();

            QueriedDataIterator queriedData = await selector.SelectFieldsOnlyAsync(items, _context.Data.AsyncQueryer);
            PageDetails pageDetails = queryBuilder.GetPageDetails(queriedData);

            return new RestCollectionData(queriedData, pageDetails);
        }

        public IRestItem GetItem(string identifier, string identifierName = null)
        {
            _context.Data.Transaction.StartTransaction();

            IIdentifierInfo<TItem> identifierInfo = _context.Identifiers.GetInfo(identifierName);
            if (identifierInfo == null)
                throw new IdentifierNotFoundException(identifierName);

            IDeferredItem<TItem> item = EngineIdentiferUtility.GetIdentifiedItem(_context, identifierInfo, identifier);

            if (!_context.AuthorizationChecker.CanGetItem(item))
                throw new NotAuthorizedForItemException(AuthorizableVerb.Get);

            return new EngineRestItem<TItem>(_context, item);
        }

        public IRestDictionary ToDictionary(string identifierName)
        {
            IIdentifierInfo<TItem> namedReferenceInfo = _context.Identifiers.GetInfo(identifierName);
            if (namedReferenceInfo == null)
                throw new RestApiException(ErrorStatus.NotFound, "An identifier named '" + identifierName + "' was not found.");

            return new EngineRestDictionary<TItem>(_context, namedReferenceInfo);
        }

        public async Task<CreatedItemAcknowledgment> AddAsync(RestItemData itemData)
        {
            if (!_context.AuthorizationChecker.CanAddItem())
                throw new NotAuthorizedForItemException(AuthorizableVerb.Add);

            _context.Data.Transaction.StartTransaction();
            await _context.Data.Repository.InitializeAsync();

            TItem newItem = _context.Data.Repository.CreateAndAttachItem();

            var setter = new QueryableFieldSetter<TItem>(_context);
            await setter.SetMappedValuesAsync(new PostedNewItem<TItem>(newItem), itemData);

            await _context.Data.Transaction.SaveChangesAsync();

            object identifier = _context.Identifiers.GetInfo(null).GetValue(newItem);
            return new CreatedItemAcknowledgment(identifier);
        }

        public async Task<Acknowledgment> DeleteAllAsync(IRestCollectionQuery query)
        {
            _context.Data.Transaction.StartTransaction();
            await _context.Data.Repository.InitializeAsync();

            var queryBuilder = new ContextQueryBuilder<TItem>(_context, query);
            IQueryable<TItem> items = queryBuilder.BuildQueryable();

            foreach (TItem item in items.ToList())
            {
                var loadedItem = new AlreadyLoadedItem<TItem>(item, string.Empty);
                
                if (!_context.AuthorizationChecker.CanDeleteItem(loadedItem))
                    throw new NotAuthorizedForItemException(AuthorizableVerb.Delete);

                _context.Data.Repository.MarkDeleted(item);
            }
            
            await _context.Data.Transaction.SaveChangesAsync();
            
            return new Acknowledgment();
        }
    }
}