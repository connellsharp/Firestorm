using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2
{
    public class EFCoreAsyncQueryer : IAsyncQueryer
    {
        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return query.ForEachAsync(action);
        }
    }
}