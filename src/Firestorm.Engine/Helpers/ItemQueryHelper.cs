using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Engine
{
    /// <summary>
    /// Contains simple helper to get a single item from the "itemQuery" i.e. an IQueryable intended only to contain 1 item.
    /// </summary>
    public static class ItemQueryHelper
    {
        public static object SingleOrThrow(IQueryable source)
        {
            if (source.IsInMemory()) // nasty fix for non-entity queries.
                return SingleOrThrow<object>(source.OfType<object>());

            if (source.ElementType.IsValueType) // executes the query here. works around that EF can't box value types.
                return SingleOrThrow(IterateObjects(source));

            return SingleOrThrow<object>(source.AsObjects());
        }

        public static T SingleOrThrow<T>(IEnumerable<T> source)
            where T : class
        {
            return SingleOrCreate(source, ThrowNotFound<T>);
        }

        public static T SingleOrCreate<T>(IEnumerable<T> source, Func<T> createAndAttachItem)
        {
            // TODO: check this only loads 2 from SQL
            IEnumerator<T> enumerator = source.Take(2).GetEnumerator();

            if (!enumerator.MoveNext())
                return createAndAttachItem();

            T returnObj = enumerator.Current;

            if (enumerator.MoveNext())
                throw new MultipleItemsMatchedReferenceException();

            return returnObj;
        }

        public static Task<object> SingleOrThrowAsync(IQueryable source, ForEachAsyncDelegate<object> forEachAsync)
        {
            if (source.IsInMemory()) // nasty fix for non-entity queries.
                return SingleOrThrowAsync(source.OfType<object>(), forEachAsync);

            if (source.ElementType.IsValueType) // executes the query here. works around that EF can't box value types.
                return Task.FromResult(SingleOrThrow(IterateObjects(source)));

            return SingleOrThrowAsync(source.AsObjects(), forEachAsync);
        }

        public static Task<T> SingleOrThrowAsync<T>(IQueryable<T> source, ForEachAsyncDelegate<T> forEachAsync)
            where T : class
        {
            return SingleOrCreateAsync(source, forEachAsync, ThrowNotFound<T>);
        }

        public static async Task<T> SingleOrCreateAsync<T>(IQueryable<T> source, ForEachAsyncDelegate<T> forEachAsync, Func<T> createAndAttachItem)
            where T : class
        {
            IQueryable<T> takeQuery = source.Take(2);

            T returnObj = null;
            int i = 0;

            await forEachAsync(takeQuery, item =>
            {
                if (++i > 1)
                    throw new MultipleItemsMatchedReferenceException();

                returnObj = item;
            });

            if (i == 0)
                return createAndAttachItem();

            return returnObj;
        }

        public static bool IsInMemory(this IQueryable source)
        {
            return source.Provider is EnumerableQuery;
        }

        public static IQueryable<object> AsObjects(this IQueryable source)
        {
            //return source.Select(o => o);
            return source.Provider.CreateQuery<object>(source.Expression);
        }

        private static IEnumerable<object> IterateObjects(IQueryable source)
        {
            foreach (object obj in source)
            {
                yield return obj;
            }
        }

        public static T ThrowNotFound<T>()
        {
            throw new ItemWithIdentifierNotFoundException();
        }

        public static Task DefaultForEachAsync<T>(IQueryable<T> source, Action<T> action)
        {
            foreach (T obj in source)
            {
                action(obj);
            }

            return Task.FromResult(false);
        }
    }
}