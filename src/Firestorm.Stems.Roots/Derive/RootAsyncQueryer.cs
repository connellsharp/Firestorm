using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;

namespace Firestorm.Stems.Roots.Derive
{
    public class RootAsyncQueryer : IAsyncQueryer
    {
        private readonly Root _root;

        public RootAsyncQueryer(Root root)
        {
            _root = root;
        }

        public Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return _root.ForEachAsync(query, action);
        }
    }
}