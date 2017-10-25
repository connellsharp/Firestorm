using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Core;

namespace Firestorm.Tests.Endpoints.Models
{
    public abstract class MemoryRestCollection<TEntity> : IRestCollection
    {
        protected abstract List<TEntity> Entities { get; }

        protected abstract string GetIdentifier(TEntity entity);

        public async Task<RestCollectionData> QueryDataAsync(IRestCollectionQuery query)
        {
            return new RestCollectionData(QueryData(query));
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
            throw new NotImplementedException("Test collection not implemented getting dictionaries");
        }

        public Task<CreatedItemAcknowledgment> AddAsync(RestItemData itemData)
        {
            throw new NotImplementedException("Test collection not implemented adding.");
        }
    }
}