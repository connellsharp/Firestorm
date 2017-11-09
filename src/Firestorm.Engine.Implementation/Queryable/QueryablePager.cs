using System.Linq;

namespace Firestorm.Engine.Queryable
{
    internal class QueryablePager<TItem>
        where TItem : class
    {
        private readonly PageInstruction _pageInstruction;

        public QueryablePager(PageInstruction pageInstruction)
        {
            _pageInstruction = pageInstruction;
        }

        public IQueryable<TItem> ApplyPagination(IQueryable<TItem> items)
        {
            // TODO need to know sort order to get page=end

            if(_pageInstruction.Offset.HasValue)
                items = items.Take(_pageInstruction.Offset.Value);

            items = items.Take(_pageInstruction.Size);

            return items;
        }
    }
}