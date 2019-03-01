using System;
using Reflectious;

namespace Firestorm
{
    /// <summary>
    /// A lightweight <see cref="IServicesBuilder"/>.
    /// Used for testing and defaults. A real application would usually use their own DI container.
    /// </summary>
    public class DefaultServicesBuilder : IServicesBuilder
    {
        private readonly ServiceFactoryDictionary _dictionary;

        public DefaultServicesBuilder()
        {
            _dictionary = new ServiceFactoryDictionary();
        }

        public IServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class
        {
            _dictionary.AddFactory(typeof(TService), implementationFactory);
            return this;
        }

        public IServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            _dictionary.AddFactory(typeof(TService), sp => implementationInstance);
            return this;
        }

        public IServicesBuilder Add(Type abstractType, Type implementationType)
        {
            _dictionary.AddFactory(abstractType, sp => CreateService(sp, implementationType));
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