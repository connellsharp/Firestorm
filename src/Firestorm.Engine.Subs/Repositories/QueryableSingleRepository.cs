using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems.Repositories
{
    /// <summary>
    /// An engine repository for query containing a single item.
    /// </summary>
    public class QueryableSingleRepository<TNav> : IEngineRepository<TNav>
        where TNav : class
    {
        public QueryableSingleRepository([NotNull] IQueryableSingle<TNav> singleItemQuery)
        {
            if (singleItemQuery == null) throw new ArgumentNullException(nameof(singleItemQuery));

            SingleItemQuery = singleItemQuery;
        }

        public IQueryableSingle<TNav> SingleItemQuery { get; }

        public Task InitializeAsync()
        {
            return Task.FromResult(false);
        }

        public IQueryable<TNav> GetAllItems()
        {
            return SingleItemQuery;
        }

        public TNav CreateAndAttachItem()
        {
            throw new NotSupportedException("Cannot add items to a single item repository.");
        }

        public void MarkDeleted(TNav item)
        {
            throw new NotSupportedException("Cannot delete items from a single item repository.");
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }
    }
}