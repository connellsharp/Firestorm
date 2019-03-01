using System;
using System.Collections.Generic;

namespace Firestorm
{
    internal class ServiceFactoryDictionary
    {
        private readonly IDictionary<Type, IList<IServiceFactory>> _dictionary;
        
        internal ServiceFactoryDictionary()
        {
            _dictionary = new Dictionary<Type, IList<IServiceFactory>>();
        }
        
        public IEnumerable<IServiceFactory> GetFactories(Type type)
        {
            return _dictionary[type];
        }

        public void AddFactory(Type type, Func<IServiceProvider, object> implementationFactory)
        {
            AddFactory(type, new CachingServiceFactory(implementationFactory));
        }

        public void AddFactory(Type type, IServiceFactory factory)
        {
            var factories = _dictionary.ContainsKey(type)
                ? _dictionary[type]
                : _dictionary[type] = new List<IServiceFactory>();
            
            factories.Add(factory);
        }

        public bool ContainsKey(Type serviceType)
        {
            return _dictionary.ContainsKey(serviceType);
        }
    }
}