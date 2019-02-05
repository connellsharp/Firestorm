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
            if (!_dictionary.ContainsKey(serviceType))
                return null;

            var factoryFuncs = _dictionary[serviceType];

            if (serviceType.IsOfGenericTypeDefinition(typeof(IEnumerable<>)))
                return factoryFuncs.Select(func => func(this));

            return factoryFuncs.Last().Invoke(this);
        }
    }
}