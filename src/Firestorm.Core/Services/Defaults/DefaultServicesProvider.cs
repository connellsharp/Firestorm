using System;
using System.Collections.Generic;
using System.Linq;
using Reflectious;

namespace Firestorm
{
    public class DefaultServicesProvider : IServiceProvider
    {
        private readonly ServiceFactoryDictionary _dictionary;

        internal DefaultServicesProvider(ServiceFactoryDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsOfGenericTypeDefinition(typeof(IEnumerable<>)))
            {
                Type innerType = serviceType.GenericTypeArguments[0];

                return Reflect.Instance(this).GetMethod(nameof(GetEnumerable)).MakeGeneric(innerType).Invoke();
            }

            if (!_dictionary.ContainsKey(serviceType)) 
                return null;
            
            var factories = _dictionary.GetFactories(serviceType);
            return factories.Last().Get(this);
        }

        private IEnumerable<T> GetEnumerable<T>()
        {
            if (!_dictionary.ContainsKey(typeof(T)))
                return Enumerable.Empty<T>();

            IEnumerable<IServiceFactory> innerFactories = _dictionary.GetFactories(typeof(T));
            return innerFactories.Select(f => f.Get(this)).OfType<T>();
        }
    }
}