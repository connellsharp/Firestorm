using System;
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
            if (_pageInstruction == null)
                return items.Take(100); // TODO default? where from?

            if (_pageInstruction.Offset.HasValue)
            {
                if (_pageInstruction.Offset < 0)
                {
                    items = items.Reverse();

                    if (_pageInstruction.Offset != 0)
                        items = items.Skip(Math.Abs(_pageInstruction.Offset.Value));

                    items = items.Take(_pageInstruction.Size);
                    items = items.Reverse();
                }
                else
                {
                    if (_pageInstruction.Offset != 0)
                        items = items.Skip(_pageInstruction.Offset.Value);

                    items = items.Take(_pageInstruction.Size);
                }
            }

            return items;
        }
    }
}