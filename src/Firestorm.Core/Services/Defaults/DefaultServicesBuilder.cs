﻿using System;
using System.Collections.Generic;
using Reflectious;

namespace Firestorm
{
    /// <summary>
    /// A lightweight <see cref="IServicesBuilder"/>.
    /// Used for testing and defaults. A real application would usually use their own DI container.
    /// </summary>
    public class DefaultServicesBuilder : IServicesBuilder
    {
        private readonly IDictionary<Type, IList<Func<IServiceProvider, object>>> _dictionary;

        public DefaultServicesBuilder()
        {
            _dictionary = new Dictionary<Type, IList<Func<IServiceProvider, object>>>();
        }

        public IServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class
        {
            GetFuncs(typeof(TService)).Add(implementationFactory);
            return this;
        }

        public IServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            GetFuncs(typeof(TService)).Add(sp => implementationInstance);
            return this;
        }

        public IServicesBuilder Add(Type abstractType, Type implementationType)
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