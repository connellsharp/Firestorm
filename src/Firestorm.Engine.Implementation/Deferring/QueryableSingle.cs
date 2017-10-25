using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Firestorm.Engine
{
    internal class QueryableSingle<T> : IQueryableSingle<T>
    {
        private readonly IQueryable<T> _underlyingQuery;

        public QueryableSingle(IQueryable<T> underlyingQuery)
        {
            _underlyingQuery = underlyingQuery;
        }

        #region Implementation of IQueryable

        public IEnumerator<T> GetEnumerator()
        {
            return _underlyingQuery.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _underlyingQuery).GetEnumerator();
        }

        public Expression Expression
        {
            get { return _underlyingQuery.Expression; }
        }

        public Type ElementType
        {
            get { return _underlyingQuery.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return _underlyingQuery.Provider; }
        }

        #endregion
    }
}