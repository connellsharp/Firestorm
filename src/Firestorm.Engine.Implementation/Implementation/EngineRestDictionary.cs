using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Engine.Queryable;

namespace Firestorm.Engine
{
    internal class EngineRestDictionary<TItem> : IRestDictionary
        where TItem : class
    {
        private const string IDENTIFIER_FIELD_NAME = "__id";
        private readonly IEngineContext<TItem> _context;
        private readonly IIdentifierInfo<TItem> _namedReferenceInfo;

        public EngineRestDictionary(IEngineContext<TItem> context, IIdentifierInfo<TItem> namedReferenceInfo)
        {
            _context = context;
            _namedReferenceInfo = namedReferenceInfo;
        }

        public async Task<RestDictionaryData> QueryDataAsync(IRestCollectionQuery query)
        {
            await _context.Repository.InitializeAsync();

            var queryBuilder = new ContextQueryBuilder<TItem>(_context, query);
            IQueryable<TItem> items = queryBuilder.BuildQueryable();

            var fieldAuth = new FieldAuthChecker<TItem>(_context.Fields, _context.AuthorizationChecker, null);
            IEnumerable<string> fields = fieldAuth.GetOrEnsureFields(query?.SelectFields, 1);

            var readers = _context.Fields.GetReaders(fields);
            readers.Add(IDENTIFIER_FIELD_NAME, new IdentifierFieldReader<TItem>(_namedReferenceInfo));
            var selector = new QueryableFieldSelector<TItem>(readers);

            QueriedDataIterator queriedData = await selector.SelectFieldsOnlyAsync(items, _context.Repository.ForEachAsync);
            PageDetails pageDetails = queryBuilder.GetPageDetails(queriedData);

            var dictionaryData = GetKeys(queriedData).ToDictionary(k => k.Key, v => (object)v.Value);

            return new RestDictionaryData(dictionaryData, pageDetails);
        }

        private static IEnumerable<KeyValuePair<string, RestItemData>> GetKeys(IEnumerable<RestItemData> queriedData)
        {
            foreach (RestItemData itemData in queriedData)
            {
                string identifier = itemData[IDENTIFIER_FIELD_NAME].ToString();
                itemData.Remove(IDENTIFIER_FIELD_NAME);
                yield return new KeyValuePair<string, RestItemData>(identifier, itemData);
            }
        }

        public IRestItem GetItem(string identifier)
        {
            _context.Transaction.StartTransaction();

            // create item using only the identifier info given to the dictionary.
            IDeferredItem<TItem> item = EngineIdentiferUtility.GetIdentifiedItem(_context, _namedReferenceInfo, identifier);

            if (!_context.AuthorizationChecker.CanGetItem(item))
                throw new NotAuthorizedForItemException(AuthorizableVerb.Get);

            return new EngineRestItem<TItem>(_context, item);
        }
    }
}