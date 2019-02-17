using System;
using System.Collections.Generic;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Analysis
{
    public class ImplementationCache : IServiceGroup
    {
        private readonly Dictionary<Type, Dictionary<Type, object>> _dictionary;

        internal ImplementationCache()
        {
            _dictionary = new Dictionary<Type, Dictionary<Type, object>>();
        }

        public void Add<T>(Type stemType, T obj)
        {
            var typedServices = _dictionary.GetOrCreate(stemType);
            typedServices.Add(typeof(T), obj);
        }

        public IServiceProvider GetProvider(Type stemType)
        {
            if(_dictionary.ContainsKey(stemType))
                throw new ArgumentException("No services are registered in the group for the stemType '" + stemType.Name + "'");

            return new ImplementationCacheServiceProvider(_dictionary[stemType]);
        }

        public class ImplementationCacheServiceProvider : IServiceProvider
        {
            private readonly Dictionary<Type, object> _dictionary;

            public ImplementationCacheServiceProvider(Dictionary<Type,object> dictionary)
            {
                _dictionary = dictionary;
            }

            public object GetService(Type serviceType)
            {
                return _dictionary[serviceType];
            }
        }
    }
}