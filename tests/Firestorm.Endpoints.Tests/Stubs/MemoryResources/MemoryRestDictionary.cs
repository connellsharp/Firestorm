using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Endpoints.Tests.Stubs.MemoryResources
{
    public class MemoryRestDictionary<TEntity> : IRestDictionary
    {
        private readonly MemoryRestCollection<TEntity> _memoryRestCollection;
        private readonly string _identifierName;

        internal MemoryRestDictionary(MemoryRestCollection<TEntity> memoryRestCollection, string identifierName)
        {
            _memoryRestCollection = memoryRestCollection;
            _identifierName = identifierName;
        }

        public async Task<RestDictionaryData> QueryDataAsync(IRestCollectionQuery query)
        {
            RestCollectionData items = await _memoryRestCollection.QueryDataAsync(query);

            return new RestDictionaryData(items.Items.Select(GetIdentifiedPair), items.PageDetails);
        }

        private KeyValuePair<string, object> GetIdentifiedPair(RestItemData item)
        {
            return new KeyValuePair<string, object>(item[_identifierName].ToString(), item);
        }

        public IRestItem GetItem(string identifier)
        {
            throw new NotImplementedException();
        }
    }
}