using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Firestorm.Engine
{
    public static class QueryableSingleExtension
    {
        public static IQueryableSingle<T> SingleDefferred<T>(this IEnumerable<T> source)
        {
            return source.AsQueryable().SingleDefferred();
        }

        public static IQueryableSingle<T> SingleDefferred<T>(this IEnumerable<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.AsQueryable().SingleDefferred(predicate);
        }

        public static IQueryableSingle<T> SingleDefferred<T>(this IQueryable<T> source)
        {
            return source as IQueryableSingle<T> ?? new QueryableSingle<T>(source);
        }

        public static IQueryableSingle<T> SingleDefferred<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            return new QueryableSingle<T>(source.Where(predicate));
        }
    }
}