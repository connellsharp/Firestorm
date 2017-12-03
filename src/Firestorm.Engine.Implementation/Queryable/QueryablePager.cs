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

        /// <summary>
        /// Returns the actual page size based on the given instructions and configuration.
        /// </summary>
        public int PageSize
        {
            get { return _pageInstruction?.Size ?? DEFAULT_PAGE_SIZE; }
        }

        /// <summary>
        /// Applies the offset and limit to the given queryable.
        /// The number of items is limited to <see cref="PageSize"/>+1.
        /// </summary>
        public IQueryable<TItem> ApplyPagination(IQueryable<TItem> items)
        {
            AbsolutePageValues absolutePageValues = GetAbsolutePageValues();

            if (absolutePageValues.EndOfPage)
                items = items.Reverse();

            if (absolutePageValues.AbsoluteOffset != 0)
                items = items.Skip(absolutePageValues.AbsoluteOffset);

            items = items.Take(absolutePageValues.PageSize + 1);

            if (absolutePageValues.EndOfPage)
                items = items.Reverse();

            return items;
        }

        private AbsolutePageValues GetAbsolutePageValues()
        {
            var values = new AbsolutePageValues();

            if (_pageInstruction == null)
            {
                values.PageSize = DEFAULT_PAGE_SIZE;
                return values;
            }

            values.PageSize = _pageInstruction.Size ?? DEFAULT_PAGE_SIZE;
            values.AbsoluteOffset = 0;
            values.EndOfPage = false;

            if (_pageInstruction.PageNumber.HasValue)
            {
                if (_pageInstruction.PageNumber == 0)
                    throw new InvalidOperationException("There is no such thing as the 0th page.");

                if (_pageInstruction.PageNumber < 0)
                    values.EndOfPage = true;

                values.AbsoluteOffset += (Math.Abs(_pageInstruction.PageNumber.Value) - 1) * values.PageSize;
            }

            if (_pageInstruction.Offset.HasValue)
            {
                if (_pageInstruction.Offset < 0)
                    values.EndOfPage = true;

                values.AbsoluteOffset += Math.Abs(_pageInstruction.Offset.Value);
            }

            return values;
        }

        private class AbsolutePageValues
        {
            public int PageSize { get; set; }
            public int AbsoluteOffset { get; set; }
            public bool EndOfPage { get; set; }
        }
    }
}