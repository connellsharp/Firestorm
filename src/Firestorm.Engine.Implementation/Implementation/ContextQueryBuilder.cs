using System.Linq;
using Firestorm.Data;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable;

namespace Firestorm.Engine
{
    /// <summary>
    /// Used by <see cref="EngineRestCollection{TItem}"/> and <see cref="EngineRestDictionary{TItem}"/>
    /// to build a query on the repository to get only requested and necessary data.
    /// </summary>
    internal class ContextQueryBuilder<TItem>
        where TItem : class
    {
        private readonly IRestCollectionQuery _query;

        public ContextQueryBuilder(IRestCollectionQuery query)
        {
            _query = query;
        }

        public IQueryable<TItem> BuildQueryable(IEngineRepository<TItem> repository, IAuthorizationChecker<TItem> authorizationChecker, IFieldProvider<TItem> fieldProvider)
        {
            IQueryable<TItem> items = repository.GetAllItems();

            items = authorizationChecker.ApplyFilter(items);

            if (_query == null)
                return items;

            var filter = new QueryableFieldFilter<TItem>(fieldProvider, _query.FilterInstructions);
            items = filter.ApplyFilter(items);

            var sorter = new QueryableFieldSorter<TItem>(fieldProvider, _query.SortIntructions);
            items = sorter.ApplySortOrder(items);

            var pager = new QueryablePager<TItem>(_query.PageInstruction);
            items = pager.ApplyPagination(items);

            return items;
        }
    }
}