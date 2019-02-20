using System;
using System.Collections.Generic;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Analysis
{
    internal class ServiceCache
    {
        private readonly Dictionary<Type, Dictionary<Type, object>> _dictionary;

        internal ServiceCache()
        {
            _dictionary = new Dictionary<Type, Dictionary<Type, object>>();
        }

        public void Add<T>(Type stemType, T obj)
        {
            var typedServices = GetDictionary(stemType);
            typedServices.Add(typeof(T), obj);
        }

        public T Get<T>(Type stemType)
        {
            var typedServices = GetDictionary(stemType);
            return (T)typedServices[typeof(T)];
        }

        public IDictionary<Type, object> GetDictionary(Type stemType)
        {
            return _dictionary.GetOrCreate(stemType);
        }

        public bool ContainsKey(Type stemType)
        {
            return _dictionary.ContainsKey(stemType);
        }
    }
}