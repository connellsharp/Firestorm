using System;
using System.Collections.Generic;

namespace Firestorm.Defaults
{
    public class DefaultServicesProvider : IFirestormServiceProvider
    {
        private readonly IDictionary<Type, Func<IFirestormServiceProvider, object>> _dictionary;

        public DefaultServicesProvider(IDictionary<Type,Func<IFirestormServiceProvider,object>> dictionary)
        {
            _dictionary = dictionary;
        }

        public object GetService(Type serviceType)
        {
            if (!_dictionary.ContainsKey(serviceType))
                return null;
            
            return _dictionary[serviceType].Invoke(this);
        }

        public IServiceProvider GetRequestServiceProvider()
        {
            return new NotImplementedServiceProvider();
        }
    }
}