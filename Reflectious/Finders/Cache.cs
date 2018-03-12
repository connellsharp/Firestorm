using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Firestorm
{
    internal class Cache<T>
    {
        private readonly ConcurrentDictionary<string, T> _dictionary = new ConcurrentDictionary<string, T>();

        public T GetOrAdd(string key, Func<T> create)
        {
            Debug.Assert(_dictionary.Count < 100, "Shouldn't really be adding this many cache items unless there's a bug.");
                
            return _dictionary.GetOrAdd(key, k => create());
        }
    }
}