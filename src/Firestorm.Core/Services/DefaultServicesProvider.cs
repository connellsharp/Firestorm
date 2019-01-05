using System;
using System.Collections.Generic;

namespace Firestorm.Owin
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
            return _dictionary[serviceType].Invoke(this);
        }

        public IServiceProvider GetRequestServiceProvider()
        {
            return new NotImplementedServiceProvider();
        }
    }
}