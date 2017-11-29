using System;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm.Engine.Queryable
{
    internal class QueryablePager<TItem>
        where TItem : class
    {
        private const int DEFAULT_PAGE_SIZE = 100; // TODO from config somewhere?

        [CanBeNull] private readonly PageInstruction _pageInstruction;

        public QueryablePager([CanBeNull] PageInstruction pageInstruction)
        {
            _pageInstruction = pageInstruction;
        }

        public int PageSize
        {
            get { return _pageInstruction?.Size ?? DEFAULT_PAGE_SIZE; }
        }

        public IQueryable<TItem> ApplyPagination(IQueryable<TItem> items)
        {
            if (_pageInstruction == null)
                return items.Take(DEFAULT_PAGE_SIZE);

            int pageSize = _pageInstruction.Size ?? DEFAULT_PAGE_SIZE;

            if (_pageInstruction.PageNumber.HasValue) // overwrites offset if both are set
                _pageInstruction.Offset = (_pageInstruction.PageNumber.Value - 1) * pageSize;

            if (_pageInstruction.Offset.HasValue)
            {
                if (_pageInstruction.Offset < 0)
                {
                    items = items.Reverse();

                    if (_pageInstruction.Offset != 0)
                        items = items.Skip(Math.Abs(_pageInstruction.Offset.Value));

                    items = items.Take(pageSize + 1);
                    items = items.Reverse();
                }
                else
                {
                    if (_pageInstruction.Offset != 0)
                        items = items.Skip(_pageInstruction.Offset.Value);

                    items = items.Take(pageSize + 1);
                }
            }
            else
            {
                items = items.Take(pageSize + 1);
            }

            return items;
        }
    }
}