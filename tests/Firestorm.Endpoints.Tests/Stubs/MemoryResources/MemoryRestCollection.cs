using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Endpoints.Tests.Stubs.MemoryResources
{
    public abstract class MemoryRestCollection<TEntity> : IRestCollection
    {
        protected abstract List<TEntity> Entities { get; }

        protected abstract string GetIdentifier(TEntity entity);

        public async Task<RestCollectionData> QueryDataAsync(IRestCollectionQuery query)
        {
            return new RestCollectionData(QueryData(query), null);
        }

        private IEnumerable<RestItemData> QueryData(IRestCollectionQuery query)
        {
            if (query?.SelectFields != null)
                throw new NotImplementedException("Test collection not implemented selecting specific fields.");

            foreach (TEntity entity in Entities)
            {
                yield return new RestItemData(entity);
            }
        }

        public IRestItem GetItem(string identifier, string identifierName = null)
        {
            TEntity entity = Entities.FirstOrDefault(e => GetIdentifier(e) == identifier);
            return new MemoryRestItem<TEntity>(entity);
        }

        public IRestDictionary ToDictionary(string identifierName)
        {
            return new MemoryRestDictionary<TEntity>(this, identifierName);
        }

        public Task<CreatedItemAcknowledgment> AddAsync(RestItemData itemData)
        {
            throw new NotImplementedException("Test collection not implemented adding.");
        }
    }
}