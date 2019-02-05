using System;
using System.Collections.Generic;
using Reflectious;

namespace Firestorm
{
    public class DefaultServicesBuilder : IFirestormServicesBuilder
    {
        private readonly IDictionary<Type, IList<Func<IServiceProvider, object>>> _dictionary;

        public DefaultServicesBuilder()
        {
            _dictionary = new Dictionary<Type, IList<Func<IServiceProvider, object>>>();
        }

        public IFirestormServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class
        {
            GetFuncs(typeof(TService)).Add(implementationFactory);
            return this;
        }

        public IFirestormServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            GetFuncs(typeof(TService)).Add(sp => implementationInstance);
            return this;
        }

        public IFirestormServicesBuilder Add(Type abstractType, Type implementationType)
        {
            GetFuncs(abstractType).Add(sp => CreateService(sp, implementationType));
            return this;
        }

        private static object CreateService(IServiceProvider serviceProvider, Type serviceType)
        {
            return Reflect.Type(serviceType)
                .GetConstructor()
                .FromServiceProvider(serviceProvider)
                .Invoke();
        }

        private IList<Func<IServiceProvider, object>> GetFuncs(Type type)
        {
            return _dictionary.ContainsKey(type)
                ? _dictionary[type]
                : _dictionary[type] = new List<Func<IServiceProvider, object>>();
        }

        public IServiceProvider Build()
        {
            return new DefaultServicesProvider(_dictionary);
        }
    }
}