using System;
using System.Collections.Generic;

namespace Firestorm.Defaults
{
    public class DefaultServicesProvider : IServiceProvider
    {
        private readonly IDictionary<Type, Func<IServiceProvider, object>> _dictionary;

        public DefaultServicesProvider(IDictionary<Type,Func<IServiceProvider,object>> dictionary)
        {
            _dictionary = dictionary;
        }

        public object GetService(Type serviceType)
        {
            if (!_dictionary.ContainsKey(serviceType))
                return null;
            
            return _dictionary[serviceType].Invoke(this);
        }
    }
}