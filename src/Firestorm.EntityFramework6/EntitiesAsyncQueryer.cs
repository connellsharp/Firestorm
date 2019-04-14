using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.EntityFramework6
{
    public class EntitiesAsyncQueryer : IAsyncQueryer
    {
        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return query.ForEachAsync(action);
        }
    }
}