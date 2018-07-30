using System;
using System.Collections.Generic;
using Firestorm.Stems;

namespace Firestorm.Tests.Functionality.Stems.Helpers
{
    internal class TestDependencyResolver : IDependencyResolver
    {
        private Dictionary<Type,object> _singletons = new Dictionary<Type, object>();

        public object Resolve(Type type)
        {
            return _singletons[type];
        }

        public void Add<T>(T singleton)
        {
            _singletons[typeof(T)] = singleton;
        }
    }
}