using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Fluent.Fuel.Engine
{
    /// <summary>
    /// A read-only dictionary that lazy loads the mappings when it is first read from.
    /// </summary>
    /// <remarks>
    /// This works around that the <see cref="FluentEngineSubContext{TItem}"/> is given an empty IEnumerable which is populated later.
    /// This means the map of field and identifier models to their Name property happens upon the first API request.
    /// </remarks>
    internal class LazyDictionary<T> : IReadOnlyDictionary<string, T>
    {
        private readonly IEnumerable<T> _items;
        private readonly Func<T, string> _getKey;
        private IReadOnlyDictionary<string, T> _dictionary;

        public LazyDictionary(IEnumerable<T> items, Func<T, string> getKey)
        {
            _items = items;
            _getKey = getKey;
        }

        private void EnsureLoaded()
        {
            if (_dictionary == null)
            {
                try
                {
                    _dictionary = _items.ToDictionary(_getKey);
                }
                catch (Exception ex)
                {
                    throw new LazyDictionaryLoadException(ex);
                }
            }
        }

        private class LazyDictionaryLoadException : Exception
        {
            public LazyDictionaryLoadException(Exception innerException)
                : base("An error occurred loading the keys for the dictionary.", innerException)
            { }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _dictionary).GetEnumerator();
        }

        public int Count
        {
            get
            {
                EnsureLoaded();
                return _dictionary.Count;
            }
        }

        public bool ContainsKey(string key)
        {
            EnsureLoaded();
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            EnsureLoaded();
            return _dictionary.TryGetValue(key, out value);
        }

        public T this[string key]
        {
            get
            {
                EnsureLoaded();
                return _dictionary[key];
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                EnsureLoaded();
                return _dictionary.Keys;
            }
        }

        public IEnumerable<T> Values
        {
            get
            {
                EnsureLoaded();
                return _dictionary.Values;
            }
        }
    }
}