using System.Linq;
using Firestorm.Engine.Queryable;
using JetBrains.Annotations;

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
        private readonly IEngineContext<TItem> _context;
        private readonly QueryablePager<TItem> _pager;

        public ContextQueryBuilder(IEngineContext<TItem> context, [CanBeNull] IRestCollectionQuery query)
        {
            _query = query;
            _context = context;

            _pager = new QueryablePager<TItem>(_query?.PageInstruction);
        }

        public IQueryable<TItem> BuildQueryable()
        {
            IQueryable<TItem> items = _context.Data.Repository.GetAllItems();

            items = _context.AuthorizationChecker.ApplyFilter(items);

            if (_query == null)
                return items;

            var filter = new QueryableFieldFilter<TItem>(_context.Fields, _query.FilterInstructions);
            items = filter.ApplyFilter(items);

            var sorter = new QueryableFieldSorter<TItem>(_context.Fields, _query.SortInstructions);
            items = sorter.ApplySortOrder(items);

            items = _pager.ApplyPagination(items);

            return items;
        }

        public PageDetails GetPageDetails([NoEnumeration] QueriedDataIterator queriedData)
        {
            if (queriedData.Length <= _pager.PageSize)
            {
                return new PageDetails
                {
                    HasNextPage = false
                };
            }

            queriedData.RemoveLastItem();

            return new PageDetails
            {
                HasNextPage = true
            };
        }
    }
}