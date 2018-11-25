using System;
using System.Collections.Generic;
using Reflectious;

namespace Firestorm.Owin
{
    public class OwinServicesBuilder : IFirestormServicesBuilder
    {
        private readonly IDictionary<Type, Func<IFirestormServiceProvider, object>> _dictionary;

        public OwinServicesBuilder()
        {
            _dictionary = new Dictionary<Type, Func<IFirestormServiceProvider, object>>();
        }

        public IFirestormServicesBuilder Add<TService>(Func<IFirestormServiceProvider, TService> implementationFactory) 
            where TService : class
        {
            _dictionary.Add(typeof(TService), implementationFactory);
            return this;
        }

        public IFirestormServicesBuilder Add(Type serviceType)
        {
            _dictionary.Add(serviceType, sp => CreateService(sp, serviceType));
            return this;
        }

        public IFirestormServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            _dictionary.Add(typeof(TService), sp => implementationInstance);
            return this;
        }

        public IFirestormServicesBuilder Add<TAbstraction, TImplementation>() 
            where TAbstraction : class where TImplementation : class, TAbstraction
        {
            _dictionary.Add(typeof(TAbstraction), sp => CreateService(sp, typeof(TImplementation)));
            return this;
        }

        private static object CreateService(IServiceProvider serviceProvider, Type serviceType)
        {
            return Reflect.Type(serviceType)
                .GetConstructor()
                .FromServiceProvider(serviceProvider)
                .Invoke();
        }

        public IFirestormServiceProvider Build()
        {
            return new OwinServicesProvider(_dictionary);
        }
    }
}