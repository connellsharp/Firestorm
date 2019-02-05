using System;
using System.Collections.Generic;
using Firestorm.Defaults;
using Reflectious;

namespace Firestorm
{
    public class DefaultServicesBuilder : IFirestormServicesBuilder
    {
        private readonly IDictionary<Type, Func<IServiceProvider, object>> _dictionary;

        public DefaultServicesBuilder()
        {
            _dictionary = new Dictionary<Type, Func<IServiceProvider, object>>();
        }

        public IFirestormServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class
        {
            _dictionary.Add(typeof(TService), implementationFactory);
            return this;
        }

        public IFirestormServicesBuilder Add(Type abstractType, Type implementationType)
        {
            _dictionary.Add(abstractType, sp => CreateService(sp, implementationType));
            return this;
        }

        public IFirestormServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            _dictionary.Add(typeof(TService), sp => implementationInstance);
            return this;
        }

        private static object CreateService(IServiceProvider serviceProvider, Type serviceType)
        {
            return Reflect.Type(serviceType)
                .GetConstructor()
                .FromServiceProvider(serviceProvider)
                .Invoke();
        }

        public IServiceProvider Build()
        {
            return new DefaultServicesProvider(_dictionary);
        }
    }
}