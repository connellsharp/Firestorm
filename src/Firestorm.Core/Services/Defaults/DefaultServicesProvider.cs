using System;
using System.Collections.Generic;
using System.Linq;
using Reflectious;

namespace Firestorm
{
    public class DefaultServicesProvider : IServiceProvider
    {
        private readonly IDictionary<Type, IList<Func<IServiceProvider, object>>> _dictionary;

        public DefaultServicesProvider(IDictionary<Type, IList<Func<IServiceProvider, object>>> dictionary)
        {
            _dictionary = dictionary;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsOfGenericTypeDefinition(typeof(IEnumerable<>)))
            {
                Type innerType = serviceType.GenericTypeArguments[0];

                return Reflect.Instance(this).GetMethod("GetEnumerable").MakeGeneric(innerType).Invoke();
            }

            if (!_dictionary.ContainsKey(serviceType)) 
                return null;
            
            var factories = _dictionary[serviceType];
            return factories.Last().Invoke(this);
        }

        private IEnumerable<T> GetEnumerable<T>()
        {
            if (!_dictionary.ContainsKey(typeof(T)))
                return Enumerable.Empty<T>();

            IList<Func<IServiceProvider, object>> innerFactories = _dictionary[typeof(T)];
            return innerFactories.Select(func => func(this)).OfType<T>();
        }
    }
}